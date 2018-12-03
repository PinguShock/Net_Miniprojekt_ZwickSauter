using System;
using System.Runtime.Serialization;

namespace AutoReservation.BusinessLayer {
    [Serializable]
    public class AutoUnavailableException : Exception {
        public AutoUnavailableException() {
        }

        public AutoUnavailableException(string message) : base(message) {
        }

        public AutoUnavailableException(string message, Exception innerException) : base(message, innerException) {
        }

        protected AutoUnavailableException(SerializationInfo info, StreamingContext context) : base(info, context) {
        }
    }
}