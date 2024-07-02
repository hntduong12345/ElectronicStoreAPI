namespace ElectronicStoreAPI.Constants
{
    public class ApiEndpointConstant
    {
        public const string RootEndPoint = "/api";
        public const string ApiVersion = "/v1";
        public const string ApiEndpoint = RootEndPoint + ApiVersion;

        public static class Combo
        {
            public const string CombosEndpoint = ApiEndpoint + "/combos";
            public const string ComboEndpoint = CombosEndpoint + "/{id}";
            public const string AvailableCombosEndpoint = CombosEndpoint + "/availability";
            public const string ComboStatusEndpoint = ComboEndpoint + "/status";
        }

        public static class Order
        {
            public const string OrdersEndpoint = ApiEndpoint + "/orders";
            public const string OrderEndpoint = OrdersEndpoint + "/{id}";
            public const string OrderStatusEndpoint = OrderEndpoint + "/status";
            public const string OrdersByUserEndpoint = OrdersEndpoint + "/account/{accountId}";
        }

        public static class Payment
        {
            public const string PaymentEndPoint = ApiEndpoint + "/payment";
            public const string PyamentReturnEndPoint = PaymentEndPoint + "/vnpay-return";
        }
    }
}
