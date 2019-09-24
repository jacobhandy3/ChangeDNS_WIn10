//Necessary Libraries
using System;
using System.Management;
using System.Net.NetworkInformation;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChangeDNSProj
{
    class Program
    {
        static void Main(string[] args)
        {
            //Declare NetManage object
            NetManage newSetting = new NetManage();
            //Ask the user which DNS server they would like to use
            Console.WriteLine("WELCOME TO CHANGING YOUR DNS!");
            Console.WriteLine("Press 1 -- Reset your DNS server to default");
            Console.WriteLine("Press 2 -- Change DNS server to Google's DNS server");
            Console.WriteLine("Press 3 -- Change DNS server to OpenDNS' DNS server(option 1)");
            Console.WriteLine("Press 4 -- Change DNS server to SafeDNS' DNS server");
            Console.WriteLine("Press 5 -- Change DNS server to DNS WATCH's DNS server");
            Console.WriteLine("Press 6 -- Change DNS server to Quad 9's DNS server");
            Console.WriteLine("Press 7 -- Change DNS server to Cloudflare's DNS server");
            Console.WriteLine("Press 8 -- Change DNS server to Level 3's DNS server");
            Console.WriteLine("Press 0 -- Exit");
            Console.Write("Submit your answer: ");
            //Take in the answer as a string for ease
            string resp = Console.ReadLine();
            //Known number of choices so I use a switch here for all the DNS servers
            switch (resp)
            {
                case "1":
                    newSetting.setDNS(null, null);
                    break;
                case "2":
                    newSetting.setDNS("8.8.8.8", resp);
                    break;
                case "3":
                    newSetting.setDNS("208.67.222.222", resp);
                    break;
                case "4":
                    newSetting.setDNS("195.46.39.39", resp);
                    break;
                case "5":
                    newSetting.setDNS("84.200.69.80", resp);
                    break;
                case "6":
                    newSetting.setDNS("9.9.9.9", resp);
                    break;
                case "7":
                    newSetting.setDNS("1.1.1.1", resp);
                    break;
                case "8":
                    newSetting.setDNS("195.46.39.39", resp);
                    break;
                default:
                    Console.WriteLine("INVALID ENTRY! TRY AGAIN!");
                    Console.ReadLine();
                    break;
            }
        }
        //class NetManage that enables network setting changes
        //needs System.Management
        class NetManage
        {
            //Method setDNS, purpose self-ex, takes a string of the NIC name and new DNS
            public void setDNS(string DNS, string loc)
            {
                //string for ManagementObject methods' paramter
                string param = "SetDNSServerSearchOrder";
                //Create a new ManagementClass with NIC name paramter
                ManagementClass mcOBJ = new ManagementClass("Win32_NetworkAdapterConfiguration");
                //Create a new ManagementObjectCollection and set to ManagementClass object method: GetInstances, no paramter
                ManagementObjectCollection mocOBJ = mcOBJ.GetInstances();
                //Loop through each instance
                foreach (ManagementObject moOBJ in mocOBJ)
                {
                    //Check if IP is enabled
                    if ((bool)moOBJ["IPEnabled"])
                    {
                            //Dont want to mess up these settings so I use a try just in case
                            try
                            {
                                //create a ManagementBaseObject to represent newDNS by the ManagementObject method getMethodParamters with param
                                ManagementBaseObject newDNS = moOBJ.GetMethodParameters(param);
                            if (newDNS != null)
                            {
                                //if not using the default DNS server
                                if (loc != null)
                                {
                                    //create the DNS server as a string array
                                    string[] s = { DNS };
                                    //set the new DNS to the desired server
                                    newDNS["DNSServerSearchOrder"] = s;
                                }
                                else
                                //if using the default DNS server
                                {
                                    //Reset the DNS server to null
                                    newDNS["DNSServerSearchOrder"] = null;
                                }
                                //Use the ManagementObject method InvokeMethod with param, desired DNS server, and null as its paramters in that order
                                //P.S. This actually sets the DNS server
                                moOBJ.InvokeMethod(param, newDNS, null);
                            }
                            }
                            //in case it doesnt work
                            catch (Exception)
                            {
                                //save me and leave
                                throw;
                            }
                        }
                    }
                }

            }
        }
    }