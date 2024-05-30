using System;
using System.Windows.Forms;
using Stripe;

namespace ELDERLY_HEALTH_MANAGEMENT_SYSTEM
{
    public partial class Form1 : Form
    {
        private const string StripePublicKey = "pk_test_51KdYapJTwcjF35OzSybWMrrTTSBRmZJlFD1N0BWM22mPl8XXZ8HKF1tzJhKHpOamsxDkfTXu3B6ju2V3C2mDq1U200ejlyLZUR";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Set your Stripe publishable key
            StripeConfiguration.ApiKey = "sk_test_51KdYapJTwcjF35OzEzCGCS85tcf66ahS54Ut0FTNyDXpaYFG3j7Pask8CObKu6t3iH1nQ6wU03F0M6MhpjmqvXSL009SRwJzFk";

            // Load Stripe Checkout form URL into the WebBrowser control
            webBrowser1.Navigate(CreateCheckoutUrl());
        }

        private string CreateCheckoutUrl()
        {
            // Replace "your_stripe_public_key" with your actual Stripe publishable key
            return $"https://checkout.stripe.com/pay/cs_live_xxxxxxxxxxxxxxx?key={StripePublicKey}";
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
          
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            StripeConfiguration.ApiKey = "sk_test_51KdYapJTwcjF35OzEzCGCS85tcf66ahS54Ut0FTNyDXpaYFG3j7Pask8CObKu6t3iH1nQ6wU03F0M6MhpjmqvXSL009SRwJzFk";

            // Load Stripe Checkout form URL into the WebBrowser control
            webBrowser1.Navigate(CreateCheckoutUrl());
        }
    }
}
