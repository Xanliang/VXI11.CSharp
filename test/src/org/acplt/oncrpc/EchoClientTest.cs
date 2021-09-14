using System.Net;
using org.acplt.oncrpc;
using System;
using org.acplt.oncrpc.apps.jportmap;

namespace tests.org.acplt.oncrpc
{
	public class EchoClientTest
	{
        public OncRpcEmbeddedPortmap epm;

        /// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
        /// <exception cref="System.IO.IOException"></exception>
        public EchoClientTest()
		{
            //
            // Diagnostic: Is a portmapper already running?
            //
            Console.Out.Write("Checking for portmap service: ");
            bool externalPortmap = OncRpcEmbeddedPortmap.isPortmapRunning
                ();
            if (externalPortmap)
            {
                Console.Out.WriteLine("A portmap service is already running.");
                throw new Exception("A portmap service is already running.");
            }
            else
            {
                Console.Out.WriteLine("No portmap service available.");
            }
            //
            // Create embedded portmap service and check whether is has sprung
            // into action.
            //
            Console.Out.Write("Creating embedded portmap instance: ");
            try
            {
                epm = new OncRpcEmbeddedPortmap();
            }
            catch (System.IO.IOException e)
            {
                Console.Out.WriteLine("ERROR: failed:");
                Console.Out.WriteLine(e.Message);
                Console.Out.WriteLine(e.StackTrace);
            }
            catch (OncRpcException e)
            {
                Console.Out.WriteLine("ERROR: failed:");
                Console.Out.WriteLine(e.Message);
                Console.Out.WriteLine(e.StackTrace);
            }
            if (!epm.embeddedPortmapInUse())
            {
                Console.Out.Write("embedded service not used: ");
            }
            else
            {
                Console.Out.Write("embedded service started: ");
            }
            if (epm.embeddedPortmapInUse() == externalPortmap)
            {
                Console.Out.WriteLine("ERROR: no service available or both.");
                return;
            }
            Console.Out.WriteLine("Passed.");

            //
            // Create a portmap client object, which can then be used to contact
            // the local ONC/RPC ServerTest test server.
            //
            OncRpcClient client = new OncRpcTcpClient(IPAddress.Loopback, unchecked((int)(0x49679)), 1, 0);
			//
			// Ping the test server...
			//
			System.Console.Out.Write("pinging test server: ");
			try
			{
				client.call(0, XdrVoid.XDR_VOID, XdrVoid.XDR_VOID
					);
			}
			catch (OncRpcException e)
			{
				System.Console.Out.WriteLine("method call failed unexpectedly:");
                System.Console.Out.WriteLine(e.StackTrace);
				System.Environment.Exit(1);
			}
			System.Console.Out.WriteLine("test server is alive.");
			//
			// Now check the echo RPC call...
			//
			string[] messages = new string[] { "Open Source", "is not yet", "another buzzword."
				 };
			checkEcho(client, messages);
			//
			// Now test AUTH_UNIX authentication. First start with an
			// invalid credential...
			//
			OncRpcClientAuth auth = new OncRpcClientAuthUnix
				("marvin", 0, 0, new int[0]);
			client.setAuth(auth);
			System.Console.Out.Write("checking AUTH_UNIX with invalid credential: ");
			try
			{
				client.call(0, XdrVoid.XDR_VOID, XdrVoid.XDR_VOID
					);
			}
			catch (OncRpcAuthenticationException ae)
			{
				if (ae.getAuthStatus() != OncRpcAuthStatus.ONCRPC_AUTH_BADCRED)
				{
					System.Console.Out.WriteLine("received wrong authentication exception with status of "
						 + ae.getAuthStatus());
                    System.Console.Out.WriteLine(ae.StackTrace);
                    System.Environment.Exit(1);
				}
			}
			catch (OncRpcException e)
			{
				System.Console.Out.WriteLine("method call failed unexpectedly:");
                System.Console.Out.WriteLine(e.StackTrace);
                System.Environment.Exit(1);
			}
			System.Console.Out.WriteLine("passed.");
			auth = new OncRpcClientAuthUnix("marvin", 42, 815, new int[0]);
			client.setAuth(auth);
			messages = new string[] { "AUTH_UNIX", "is like", "*NO* authentication", "--", "it"
				, "uses", "*NO CRYPTOGRAPHY*", "for securing", "ONC/RPC messages" };
			checkEcho(client, messages);
			client.setAuth(null);
			//
			// Release resources bound by ONC/RPC client object as soon as possible
			// so might help the garbage wo/man. Yeah, this is now a political
			// correct comment.
			//
			client.close();
			client = null;
		}

		public virtual void checkEcho(OncRpcClient client, string[] messages
			)
		{
			for (int idx = 0; idx < messages.Length; ++idx)
			{
				XdrString @params = new XdrString(messages[idx]
					);
				XdrString result = new XdrString();
				System.Console.Out.Write("checking echo: ");
				try
				{
					client.call(1, @params, result);
				}
				catch (OncRpcException e)
				{
					System.Console.Out.WriteLine("method call failed unexpectedly:");
                    System.Console.Out.WriteLine(e.StackTrace);
					System.Environment.Exit(1);
				}
				if (!@params.stringValue().Equals(result.stringValue()))
				{
					Console.Out.WriteLine("answer does not match call:");
					Console.Out.WriteLine("  expected: \"" + @params.stringValue() + "\"");
					Console.Out.WriteLine("  but got:  \"" + result.stringValue() + "\"");
					System.Environment.Exit(1);
				}
				System.Console.Out.WriteLine(result.stringValue());
			}
		}

		public static void Main(string[] args)                                                          
		{
			Console.Out.WriteLine("EchoClientTest");
			try
			{
				new tests.org.acplt.oncrpc.EchoClientTest();
			}
			catch (System.Exception e)
			{
                Console.Out.WriteLine(e.Message);
                Console.Out.WriteLine(e.StackTrace);
			}
		}
	}
}
