/*******************************************************************************
 *******************************************************************************
 **
 ** File         visaBufferedMeasurement.cs
 ** 
 ** Copyright    (c) Rohde & Schwarz GmbH & Co. KG, Munich
 **
 ** Author       Juergen D. Geltinger, 1GS4
 ** 
 ** Responsible  JDG
 **
 ** Language     C#
 **
 ** Description  Example for using the NRP VISA Passport driver
 **              to demonstrate a buffered PowerAverage Measurement
 **
 ** Version      2015-08-26  JDG  Initial version
 **
 *******************************************************************************
 ******************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Visa32;

namespace Visa32
{
  class Program
  {
    private uint numBufferedMeas = 5; // This is the number of measurements which shall be collected...
    private bool bVerbose = false;    // Set to true to get debug output

    // Bits in Status Byte (via viReadSTB() call)
    public const uint ErrQBit = 1 << 2;
    public const uint MAVBit = 1 << 4;
    public const uint OperBit = 1 << 7;

    // Bits in Operation Condition Register (via STAT:OPER:COND query)
    public const uint MEASURING = 1 << 4;
    public const uint WAIT_FOR_TRIGGER = 1 << 5;

    //=============================================
    public uint getNumMeas
    {
      get { return numBufferedMeas; }
    }

    //=============================================
    public bool isVerbose
    {
      get { return bVerbose; }
    }

    //=============================================
    public void PrintError(int err)
    {
      if (err != VISA.VI_SUCCESS)
        Console.WriteLine("Error: 0x{0:X}", err);
    }

    //=============================================
    public void PrintStatus( int session, int err)
    {
      StringBuilder strDesc = new StringBuilder(500);

      if (err == VISA.VI_SUCCESS)
        return;

      VISA.viStatusDesc( session, err, strDesc );

      Console.WriteLine("Error: 0x{0:X}: {1}", err, strDesc);
    }

    //=============================================
    public int ReadSTB(int session, ref short statusByte)
    {
      int rc = VISA.VI_SUCCESS;
      do
      {
        rc = VISA.viReadSTB(session, ref statusByte);
        if (rc != VISA.VI_SUCCESS)
          break;

        if (bVerbose)
        {
          if ((statusByte & ErrQBit) == ErrQBit)
            Console.WriteLine("STB: Error queue not empty");

          if ((statusByte & MAVBit) == MAVBit)
            Console.WriteLine("STB: Message Available");

          if ((statusByte & OperBit) == OperBit)
            Console.WriteLine("STB: Sensor not IDLE");
          else
            Console.WriteLine("STB: Sensor in IDLE state");
        }
      } while (false);

      if (bVerbose)
        Console.WriteLine("");

      return rc;
    }


    //=============================================
    public int GetSensorState(int session, ref long State)
    {
      int rc = VISA.VI_SUCCESS;

      do 
      {
        //               upper-/lowercase is allowed
        string condCmd = "sTatUs:opeR:CondITion?\n";
        int retCnt = 0;
        int status = VISA.viWrite(session, condCmd, condCmd.Length, out retCnt);
        if (bVerbose)
          PrintStatus(session, status);
        if (rc != VISA.VI_SUCCESS)
          break;

        StringBuilder msg = new System.Text.StringBuilder(256);
        retCnt = 0;
        status = VISA.viRead(session, msg, 256, out retCnt);
        if (bVerbose)
          PrintStatus(session, status);
        if (rc != VISA.VI_SUCCESS)
          break;

        State = Int32.Parse(msg.ToString());

        if (bVerbose)
          Console.WriteLine("STAT:OPER:COND='{0}' --> Sensor State = 0x{1:X}", msg, State);

      } while (false);

      return rc;
    }

    //=============================================
    public void ReadDeviceList(int mainSession)
    {
      string        searchString = "RSNRP::?*";
      int           findList     = 0;
      int           entryCount   = 0;
      int           status       = VISA.VI_SUCCESS;
      StringBuilder strDev       = new System.Text.StringBuilder(256);

      Console.WriteLine("");
      Console.WriteLine("------------------------------------------------------------");
      Console.WriteLine("R&S NRP USB power sensors...");
     
      do 
      {
        status = VISA.viFindRsrc(mainSession, searchString, out findList, out entryCount, strDev);
        if (bVerbose)
          PrintStatus(mainSession, status);
        if (status != VISA.VI_SUCCESS)
          break;

        Console.WriteLine("| {0}", strDev);

        while (status == VISA.VI_SUCCESS)
        {
          status = VISA.viFindNext(findList, strDev);
          if (bVerbose)
            PrintStatus(mainSession, status);
          if (status != VISA.VI_SUCCESS)
            break;

          Console.WriteLine("| {0}", strDev);
        }

      } while(false);

      if ( entryCount < 1 )
        Console.WriteLine("No devices found");

      Console.WriteLine("------------------------------------------------------------");
      Console.WriteLine("");
    }

    //=============================================
    public int OpenFirstNrpSensor(int mainSession, out int pSession)
    {
      string        searchString = "RSNRP::?*";
      int           findList     = 0;
      int           entryCount   = 0;
      int           timeout      = 5000;
      int           status       = VISA.VI_SUCCESS;
      StringBuilder strDev       = new System.Text.StringBuilder(256);

      pSession = 0;

      status = VISA.viFindRsrc( mainSession, searchString, out findList, out entryCount, strDev );

      while ( status == VISA.VI_SUCCESS )
      {
        // Try to use the device (given in 'strDev')
        status = VISA.viOpen(mainSession, strDev.ToString(), VISA.VI_NO_LOCK, timeout, out pSession);

        if (status == VISA.VI_SUCCESS)
        {
          Console.WriteLine("| Opened '{0}' successfully", strDev);
          break;      // viOpen was successful --> return
        }

        if (status == VISA.VI_ERROR_RSRC_LOCKED)
        {
          Console.WriteLine("| Cannot open '{0}' -- possibly is in use", strDev);

          // Cannot use  *this*  device (may already be in use elsewhere)
          // therefore we try the next device (if any)...
          status = VISA.viFindNext(findList, strDev);

          // An error here may signify
          //   a) end-of-list  or
          //   b) viFindNext() not supported
          if ( status != VISA.VI_SUCCESS )
            break;    
        }

        // continue trying to open the device...

      }

      return status;
    }
  };


  //============================================================
  // Sample application for NRP Visa Passport
  //============================================================
  class CApp
  {
    static void Main(string[] args)
    {

      Program prg = new Program();

      Console.WriteLine("============================================================");
      Console.WriteLine("R&S NRP Visa Passport Example for Buffered Measurement");
      Console.WriteLine("   (doing a sequence of {0} single measurements)", prg.getNumMeas );
      Console.WriteLine("============================================================");

      int mainSession = 0;
      int nrpzSession = 0;
      int status = VISA.VI_SUCCESS;
      int timeout = 20000;
      int retCnt = 0;

      // Commands will will be used...
      string Cmd_Idn            = "*IDN?\n";
      string Cmd_Rst            = "*RST\n";
      string Cmd_TrigSource_Bus = "trig:sour bus\n";
      string Cmd_BufSize        = "sens:pow:avg:buff:size {0}; sens:pow:avg:buff:stat on\n";
      string Cmd_TrigCount      = "trig:count {0}\n";
      string Cmd_Avg_Off        = "sens:aver:count:auto off\n";
      string Cmd_TriggerImm     = "trig:imm\n";
      string Cmd_InitImm        = "init:imm\n";
      string Cmd_Fetch          = "FETC?\n";
      string Cmd_SystErr        = "syst:error?\n";
      string strIO;

      System.Text.StringBuilder msg = new System.Text.StringBuilder(2048);

      do
      {
        status = VISA.viOpenDefaultRM(out mainSession);
        if (status != VISA.VI_SUCCESS)
          break;

        prg.ReadDeviceList(mainSession);

        status = prg.OpenFirstNrpSensor(mainSession, out nrpzSession);
        if (status != VISA.VI_SUCCESS)
        {
          // There might be no R&S NRP USB power sensor at all
          Console.WriteLine("Could not open a device");
          Console.WriteLine("");

          break;
        }


        // *RST
        status = VISA.viWrite(nrpzSession, Cmd_Rst, Cmd_Rst.Length, out retCnt);
        if (prg.isVerbose)
          prg.PrintStatus(nrpzSession, status);
        if (status != VISA.VI_SUCCESS)
          break;


        // *IDN?
        status = VISA.viWrite(nrpzSession, Cmd_Idn, Cmd_Idn.Length, out retCnt);
        if (prg.isVerbose)
          prg.PrintStatus(nrpzSession, status);
        if (status != VISA.VI_SUCCESS)
          break;

        status = VISA.viRead(nrpzSession, msg, 2047, out retCnt);
        if (prg.isVerbose)
          prg.PrintStatus(nrpzSession, status);
        if (status != VISA.VI_SUCCESS)
          break;

        Console.WriteLine("IDN string: {0}", msg.ToString());
        Console.WriteLine("");


        // Setting Timeout
        status = VISA.viSetAttribute(nrpzSession, VISA.VI_ATTR_TMO_VALUE, timeout);
        if (prg.isVerbose)
          prg.PrintStatus(nrpzSession, status);
        if (status != VISA.VI_SUCCESS)
          break;
        if (prg.isVerbose)
          Console.WriteLine("Timeout set to {0:F1} s", 0.001 * timeout);


        // Sensor Setup...

        // Auto Averaging off
        status = VISA.viWrite(nrpzSession, Cmd_Avg_Off, Cmd_Avg_Off.Length, out retCnt);
        if (prg.isVerbose)
          prg.PrintStatus(nrpzSession, status);
        if (status != VISA.VI_SUCCESS)
          break;

        // Trigger Source
        status = VISA.viWrite(nrpzSession, Cmd_TrigSource_Bus, Cmd_TrigSource_Bus.Length, out retCnt);
        if (prg.isVerbose)
          prg.PrintStatus(nrpzSession, status);
        if (status != VISA.VI_SUCCESS)
          break;

        // Size of measurement buffer
        strIO = string.Format(Cmd_BufSize, prg.getNumMeas);
        status = VISA.viWrite(nrpzSession, strIO, strIO.Length, out retCnt);
        if (prg.isVerbose)
          prg.PrintStatus(nrpzSession, status);
        if (status != VISA.VI_SUCCESS)
          break;

        // Trigger Count (same as measurement buffer size)
        strIO = string.Format(Cmd_TrigCount, prg.getNumMeas);
        status = VISA.viWrite(nrpzSession, strIO, strIO.Length, out retCnt);
        if (prg.isVerbose)
          prg.PrintStatus(nrpzSession, status);
        if (status != VISA.VI_SUCCESS)
          break;

      } while (false);


      if (status != VISA.VI_SUCCESS)
      {
        VISA.viClose(nrpzSession);
        prg.PrintError(status);
        Console.WriteLine("");
        Console.WriteLine("Setup error -- Hit 'Enter' to Exit");
        Console.ReadKey();

        return;
      }

      // Setup was okay -- Let's continue...

      long  lSensorState = -1;
      short uiSTB        = 0;

      do 
      {
        status = VISA.viWrite(nrpzSession, Cmd_InitImm, Cmd_InitImm.Length, out retCnt);
        if (prg.isVerbose)
          prg.PrintStatus(nrpzSession, status);
        if (status != VISA.VI_SUCCESS)
          break;
        if (prg.isVerbose)
          Console.WriteLine(">>>>>>>>>>>>> {0}", Cmd_InitImm);

        // Sensor is in WAIT_FOR_TRIGGER state now



        // Now we're going to send 'TRIG:IMM' commands
        // until the configured buffer is filled up and
        // the sensor goes to IDLE
        for ( ; ; ) 
        {
          status = prg.ReadSTB( nrpzSession, ref uiSTB );
          if (prg.isVerbose)
            prg.PrintStatus(nrpzSession, status);
          if (status != VISA.VI_SUCCESS)
            break;
          if ( (uiSTB & Program.MAVBit ) != 0 )
            break;

          // TRIG:IMM
          status = VISA.viWrite(nrpzSession, Cmd_TriggerImm, Cmd_TriggerImm.Length, out retCnt);
          if (prg.isVerbose)
            prg.PrintStatus(nrpzSession, status);
          if (status != VISA.VI_SUCCESS)
            break;
          if (prg.isVerbose)
            Console.WriteLine(">>>>>>>>>>>>> {0}", Cmd_TriggerImm);


          // Sensor is in MEASURING state now
          //
          // Let's wait for the end of measurement
          do
          {
            status = prg.GetSensorState( nrpzSession, ref lSensorState );
            Thread.Sleep(10);
          } while ((lSensorState & Program.MEASURING) == Program.MEASURING);  // while measuring

        } // forever


        // If we have a result...
        if ( (uiSTB & Program.MAVBit) != 0 )
        {
          do 
          {
            // Fetch?
            status = VISA.viWrite(nrpzSession, Cmd_Fetch, Cmd_Fetch.Length, out retCnt);
            if (prg.isVerbose)
              prg.PrintStatus(nrpzSession, status);
            if (status != VISA.VI_SUCCESS)
              break;

            status = VISA.viRead(nrpzSession, msg, 2047, out retCnt);
            if (prg.isVerbose)
              prg.PrintStatus(nrpzSession, status);
            if (status != VISA.VI_SUCCESS)
              break;

            Console.WriteLine("");
            Console.WriteLine("Result(s): {0}", msg.ToString());
            Console.WriteLine("");

          } while (false);
        }

        prg.ReadSTB( nrpzSession, ref uiSTB );
        prg.GetSensorState( nrpzSession, ref lSensorState );


        // Syst:err?
        status = VISA.viWrite(nrpzSession, Cmd_SystErr, Cmd_SystErr.Length, out retCnt);
        if (prg.isVerbose)
          prg.PrintStatus(nrpzSession, status);
        if (status != VISA.VI_SUCCESS)
          break;

        status = VISA.viRead(nrpzSession, msg, 2047, out retCnt);
        if (prg.isVerbose)
          prg.PrintStatus(nrpzSession, status);
        if (status != VISA.VI_SUCCESS)
          break;

        if ( retCnt > 0 )
        {
          Console.WriteLine("{0}", msg.ToString());
          Console.WriteLine("");
          Console.WriteLine("");
        }
      }
      while ( false );


      VISA.viClose(nrpzSession);
      prg.PrintError(status);
      Console.WriteLine("");
      Console.WriteLine("Hit 'Enter' to Exit");
      Console.ReadKey();

    }    
  }
}
