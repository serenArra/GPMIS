using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace MFAE.Jobs.Dto
{
    public class DisplayNameDto
    {
        public int Id { get; set; }

        public string NameAr { get; set; }

        public string NameEn { get; set; }

        public string DisplayName
        {
            get
            {
                if (CultureInfo.CurrentUICulture.Name == "ar")
                {
                    return NameAr;
                }
                else
                {
                    return NameEn;
                }

            }
            set
            {
                if (CultureInfo.CurrentUICulture.Name == "ar")
                {
                    NameAr = value;
                }
                else
                {
                    NameEn = value;
                }
            }
        }
    }
}
