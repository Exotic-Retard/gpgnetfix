namespace GPG.Multiplayer.Quazal
{
    using GPG;
    using System;
    using System.Runtime.InteropServices;

    public static class QuazalErrorCodes
    {
        public static QuazalError GetError()
        {
            QuazalError error = new QuazalError();
            uint num = LastReturnCode();
            if (num != 0)
            {
                string str = Loc.Get("Unknown");
                string str2 = Loc.Get("Unknown");
                byte[] bytes = BitConverter.GetBytes(num);
                error.facilitycode = bytes[2];
                error.errorcode = bytes[0];
                switch (error.facilitycode)
                {
                    case 1:
                        str = Loc.Get("<LOC>Core");
                        switch (error.errorcode)
                        {
                            case 1:
                                str2 = Loc.Get("<LOC>Could not connect to GPGnet: Supreme Commander.  Please check your internet connection and try again.");
                                break;

                            case 2:
                                str2 = Loc.Get("<LOC>The operation is currently not implemented.");
                                break;

                            case 3:
                                str2 = Loc.Get("<LOC>The operation specifies or accesses an invalid pointer.");
                                break;

                            case 4:
                                str2 = Loc.Get("<LOC>The operation was aborted.");
                                break;

                            case 5:
                                str2 = Loc.Get("<LOC>The operation raised an exception.");
                                break;

                            case 6:
                                str2 = Loc.Get("<LOC>An attempt was made to access data in an incorrect manner. This may be due to inadequate permission or the data, file, etc. not existing.");
                                break;

                            case 7:
                                str2 = Loc.Get("<LOC>The operation specifies or accesses an invalid DOHandle.");
                                break;

                            case 8:
                                str2 = Loc.Get("<LOC>The operation specifies or accesses an invalid index.");
                                break;

                            case 9:
                                str2 = Loc.Get("<LOC>The system could not allocate or access enough memory or disk space to perform the specified operation.");
                                break;

                            case 10:
                                str2 = Loc.Get("<LOC>Invalid argument were passed with the operation. The argument(s) may be out of range or invalid.");
                                break;

                            case 11:
                                str2 = Loc.Get("<LOC>The operation did not complete within the specified timeout for that operation.");
                                break;

                            case 12:
                                str2 = Loc.Get("<LOC>Initialization of the component failed.");
                                break;

                            case 13:
                                str2 = Loc.Get("<LOC>The call failed to initialize.");
                                break;

                            case 14:
                                str2 = Loc.Get("<LOC>An error occurred during registration.");
                                break;

                            case 15:
                                str2 = Loc.Get("<LOC>The buffer is too large to be sent.");
                                break;
                        }
                        break;

                    case 2:
                        str = Loc.Get("<LOC>DDL");
                        break;

                    case 3:
                        str = Loc.Get("<LOC>RendezVous");
                        switch (error.errorcode)
                        {
                            case 1:
                                str2 = Loc.Get("<LOC>Connection was unable to be established, either with the Rendez-Vous back end or a Peer.");
                                break;

                            case 2:
                                str2 = Loc.Get("<LOC>The Principal could not be authenticated by the Authentication Service.");
                                break;

                            case 100:
                                str2 = Loc.Get("<LOC>Could not find that account.  Please try again.");
                                break;

                            case 0x65:
                                str2 = Loc.Get("<LOC>The password was not correct.  Please try again.");
                                break;

                            case 0x66:
                                str2 = Loc.Get("<LOC>The provided user name already exists in the database. All usernames must be unique.");
                                break;

                            case 0x67:
                                str2 = Loc.Get("<LOC>The Principal's account still exists in the database but the account has been disabled. ");
                                break;

                            case 0x68:
                                str2 = Loc.Get("<LOC>The Principal's account still exists in the database but the account has expired.");
                                break;

                            case 0x69:
                                str2 = Loc.Get("<LOC>That account is already logged in.");
                                break;

                            case 0x6a:
                                str2 = Loc.Get("<LOC>Data encryption failed.");
                                break;

                            case 0x6b:
                                str2 = Loc.Get("<LOC>The operation specifies or accesses an invalid PrincipalID.");
                                break;

                            case 0x6c:
                                str2 = Loc.Get("<LOC>Maximum connnection number is reached");
                                break;
                        }
                        break;

                    case 4:
                        str = Loc.Get("<LOC>PythonCore");
                        switch (error.errorcode)
                        {
                            case 1:
                                str2 = Loc.Get("<LOC>The operation raised an exception.");
                                break;

                            case 2:
                                str2 = Loc.Get("<LOC>The operation was applied to an object of inappropriate type.");
                                break;

                            case 3:
                                str2 = Loc.Get("<LOC>A sequence subscript is out of range.");
                                break;

                            case 4:
                                str2 = Loc.Get("<LOC>The operation specifies or accesses an invalid reference.");
                                break;

                            case 5:
                                str2 = Loc.Get("<LOC>The call failed to complete.");
                                break;

                            case 6:
                                str2 = Loc.Get("<LOC>There was not enough memory available to complete the operation.");
                                break;

                            case 7:
                                str2 = Loc.Get("<LOC>The mapping (dictionary) key was not found in the set of existing keys.");
                                break;

                            case 8:
                                str2 = Loc.Get("<LOC>The operation raised an error.");
                                break;

                            case 9:
                                str2 = Loc.Get("<LOC>Conversion of Python values to or from another language produced an error.");
                                break;

                            case 10:
                                str2 = Loc.Get("<LOC>The caller of the operation does not have the necessary permission to perform the operation.");
                                break;
                        }
                        break;

                    case 5:
                        str = Loc.Get("<LOC>Transport");
                        switch (error.errorcode)
                        {
                            case 2:
                                str2 = Loc.Get("<LOC>Network connection was unable to be established.");
                                break;

                            case 3:
                                str2 = Loc.Get("<LOC>The URL contained in the StationURL is invalid.  The syntax may be incorrect.");
                                break;

                            case 4:
                                str2 = Loc.Get("<LOC>The key used to authenticate a given station is invalid. The secure transport layer uses secret-key based cryptography to ensure the integrity and confidentiality of data sent across the network.");
                                break;

                            case 5:
                                str2 = Loc.Get("<LOC>The specified transport type is invalid.");
                                break;

                            case 6:
                                str2 = Loc.Get("<LOC>The Station is already connected via another EndPoint.");
                                break;

                            case 7:
                                str2 = Loc.Get("<LOC>The data could not be sent across the network. This could be due to an invalid message, packet, or buffer.");
                                break;

                            case 8:
                                str2 = Loc.Get("<LOC>The operation did not complete within the specified timeout for that operation.");
                                break;

                            case 9:
                                str2 = Loc.Get("<LOC>The network connection was reset.");
                                break;

                            case 10:
                                str2 = Loc.Get("<LOC>The destination Station did not authenticate itself properly.");
                                break;

                            case 11:
                                str2 = Loc.Get("<LOC>3rd-party server or device answered with an error code according to protocol used e.g. HTTP error code");
                                break;
                        }
                        break;

                    case 6:
                        str = Loc.Get("<LOC>DOCore");
                        break;
                }
                error.message = error.facilitycode.ToString() + " " + str + ": " + error.errorcode.ToString() + " " + str2;
            }
            return error;
        }

        [DllImport("MultiplayerBackend.dll", CallingConvention=CallingConvention.Cdecl)]
        private static extern uint LastReturnCode();
    }
}

