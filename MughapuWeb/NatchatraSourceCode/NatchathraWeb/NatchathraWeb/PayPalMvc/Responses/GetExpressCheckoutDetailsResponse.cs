using PayPalMvc.Enums;
namespace PayPalMvc {
	/// <summary>
	/// Response received from a checkout details request
	/// </summary>
    public class GetExpressCheckoutDetailsResponse : CommonPaymentResponse
    {
        // PayPal Response properties
        public CheckoutStatus CHECKOUTSTATUS { get; set; }
        public string PAYMENTREQUEST_0_INVNUM { get; set; } // Should be the same as the baskedId we passed in as the tracking reference for the transaction registration
        public string PAYERSTATUS { get; set; }
        public string PAYERID { get; set; } // Stored
        public string EMAIL { get; set; }
        public string SALUTATION { get; set; }
        public string FIRSTNAME { get; set; }
        public string MIDDLENAME { get; set; }
        public string LASTNAME { get; set; }

        public string PAYMENTREQUEST_0_SHIPTONAME { get; set; }
        public string PAYMENTREQUEST_0_SHIPTOSTREET { get; set; }
        public string PAYMENTREQUEST_0_SHIPTOCITY { get; set; }
        public string PAYMENTREQUEST_0_SHIPTOSTATE { get; set; }
        public string PAYMENTREQUEST_0_SHIPTOCOUNTRYCODE { get; set; }
        public string PAYMENTREQUEST_0_SHIPTOZIP { get; set; }
        
        // Human Readable re-mapped properties
        public string TrackingReference { get { return PAYMENTREQUEST_0_INVNUM; } } // Stored

        public string ToString // Stored
        {
            get
            {
                return string.Format("Checkout Status: [{0}] Payer Status: [{1}] Name: [{2} {3} {4} {5}] PayPal Email: [{6}]  Address:[{7} {8} {9} {10} {11} {12}]", CHECKOUTSTATUS.ToString(), PAYERSTATUS, SALUTATION, FIRSTNAME, MIDDLENAME, LASTNAME, EMAIL, PAYMENTREQUEST_0_SHIPTONAME, PAYMENTREQUEST_0_SHIPTOSTREET, PAYMENTREQUEST_0_SHIPTOCITY, PAYMENTREQUEST_0_SHIPTOSTATE, PAYMENTREQUEST_0_SHIPTOCOUNTRYCODE, PAYMENTREQUEST_0_SHIPTOZIP);
            }
        }
	}
}