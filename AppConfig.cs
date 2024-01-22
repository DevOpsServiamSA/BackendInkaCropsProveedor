namespace ProveedorApi;
public class AppConfig
{
    public class Mensajes
    {
        public static string AsuntoSolicitudAcceso { get; set; } = null!;
        public static string MensajeSolicitudAcceso { get; set; } = null!;
        public static string AsuntoBloqueoPorDeuda { get; set; } = null!;
        public static string MensajeBloqueoPorDeuda { get; set; } = null!;
        public static string AsuntoDesbloqueoPorDeuda { get; set; } = null!;
        public static string MensajeDesbloqueoPorDeuda { get; set; } = null!;
        public static string AsuntoRechazoRequisitos { get; set; } = null!;
        public static string MensajeRechazoRetencion { get; set; } = null!;
        public static string MensajeRechazoDetraccion { get; set; } = null!;
        public static string AsuntoRechazoSustento { get; set; } = null!;
        public static string MensajeRechazoRequisitos { get; set; } = null!;
        public static string MensajeRechazoSustento { get; set; } = null!;
        public static string AsuntoReseteoClave { get; set; } = null!;
        public static string MensajeReseteoClave { get; set; } = null!;
        public static string MensajeSolicitudAccesoAdmin { get; set; } = null!;
        public static string AsuntoSolicitudAccesoAdmin { get; set; } = null!;
    }

    public class Configuracion
    {
        public static string Website { get; set; } = null!;
        public static string CarpetaArchivos { get; set; } = null!;
        public static string ServidorMail { get; set; } = null!;
        public static int PuertoMail { get; set; }
        public static bool EnableSSLMail { get; set; }
        public static string UserMail { get; set; } = null!;
        public static string PasswordMail { get; set; } = null!;
        public static string DestinoRobotMail { get; set; } = null!;
        public static string DestinoCompraMail { get; set; } = null!;
    }
}