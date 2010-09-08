namespace GPG.Security
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    public class Hash
    {
        private HashAlgorithm mCryptoService;
        private string mSalt;

        public Hash()
        {
            this.mCryptoService = new MD5CryptoServiceProvider();
        }

        public Hash(ServiceProviderEnum serviceProvider)
        {
            switch (serviceProvider)
            {
                case ServiceProviderEnum.SHA1:
                    this.mCryptoService = new SHA1Managed();
                    return;

                case ServiceProviderEnum.SHA256:
                    this.mCryptoService = new SHA256Managed();
                    return;

                case ServiceProviderEnum.SHA384:
                    this.mCryptoService = new SHA384Managed();
                    return;

                case ServiceProviderEnum.SHA512:
                    this.mCryptoService = new SHA512Managed();
                    return;

                case ServiceProviderEnum.MD5:
                    this.mCryptoService = new MD5CryptoServiceProvider();
                    return;
            }
        }

        public Hash(string serviceProviderName)
        {
            try
            {
                this.mCryptoService = (HashAlgorithm) CryptoConfig.CreateFromName(serviceProviderName.ToUpper());
            }
            catch
            {
                throw;
            }
        }

        public virtual string Encrypt(string plainText)
        {
            byte[] inArray = this.mCryptoService.ComputeHash(Encoding.ASCII.GetBytes(plainText + this.mSalt));
            return Convert.ToBase64String(inArray, 0, inArray.Length);
        }

        public byte[] EncryptByte(string plainText)
        {
            return this.mCryptoService.ComputeHash(Encoding.ASCII.GetBytes(plainText + this.mSalt));
        }

        public string Salt
        {
            get
            {
                return this.mSalt;
            }
            set
            {
                this.mSalt = value;
            }
        }

        public enum ServiceProviderEnum
        {
            SHA1,
            SHA256,
            SHA384,
            SHA512,
            MD5
        }
    }
}

