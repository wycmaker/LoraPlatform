//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace LoraPlatform.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    public partial class ConnectTable
    {
        public int Id { get; set; }
        [DisplayName("裝置Id")]
        public string DeviceId { get; set; }
        [DisplayName("配戴者")]
        public string Elder { get; set; }
        [DisplayName("連接帳號")]
        public string Account { get; set; }
    }
}
