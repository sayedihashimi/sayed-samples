public static class Program2 {
    public static void Main2() {
        var handler = new HttpClientHandler {
            ServerCertificateCustomValidationCallback = (_, __, ___, ____) => true
        };
    }
}

