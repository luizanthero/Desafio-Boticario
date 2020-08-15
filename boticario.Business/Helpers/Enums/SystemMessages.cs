namespace boticario.Helpers.Enums
{
    public class MessageError
    {
        public string Value { get; private set; }

        private MessageError(string value) { Value = value; }

        public static MessageError NotFound { get { return new MessageError("Nenhum registro cadastrado!"); } }
        public static MessageError NotFoundSingle { get { return new MessageError("Registro não encontrado!"); } }
        public static MessageError InternalError { get { return new MessageError("Erro interno do servidor!"); } }
        public static MessageError BadRequest { get { return new MessageError("Informação enviada inválida!"); } }
        public static MessageError UserPasswordInvalid { get { return new MessageError("Email ou Senha incorretos!"); } }
        public static MessageError DifferentIds { get { return new MessageError("Id do Obejto Json e do Paramêtro precisam ser iguais!"); } }
        public static MessageError PasswordNullorEmpty { get { return new MessageError("Senha é um campo obrigatório!"); } }
    }

    public class MessageSuccess
    {
        public string Value { get; private set; }

        private MessageSuccess(string value) { Value = value; }

        public static MessageSuccess Update { get { return new MessageSuccess("Registro atualizado com sucesso!"); } }
        public static MessageSuccess Delete { get { return new MessageSuccess("Registro excluído com sucesso!"); } }
    }

    public class MessageLog
    {
        public string Value { get; private set; }

        public MessageLog(string value) { Value = value; }

        public static MessageLog Start { get { return new MessageLog("Iniciando execução..."); } }
        public static MessageLog Stop { get { return new MessageLog("Execução finalizada!"); } }

        public static MessageLog Saving { get { return new MessageLog("Salvando registro na Base de Dados..."); } }
        public static MessageLog Saved { get { return new MessageLog("Registro salvo com sucesso!"); } }

        public static MessageLog Updating { get { return new MessageLog("Atualizando registro na Base de Dados..."); } }
        public static MessageLog UpdateNotFound { get { return new MessageLog(MessageError.NotFoundSingle.Value); } }
        public static MessageLog Updated { get { return new MessageLog(MessageSuccess.Update.Value); } }

        public static MessageLog Deleting { get { return new MessageLog("Excluindo registro na Base de Dados..."); } }
        public static MessageLog DeleteNotFound { get { return new MessageLog(MessageError.NotFoundSingle.Value); } }
        public static MessageLog Deleted { get { return new MessageLog(MessageSuccess.Delete.Value); } }

        public static MessageLog Getting { get { return new MessageLog("Buscado registro na Base de Dados..."); } }
        public static MessageLog GettingList { get { return new MessageLog("Buscando lista de registros na Base de Dados..."); } }
        public static MessageLog Getted { get { return new MessageLog("Registro(s) encontrado!"); } }

        public static MessageLog Error { get { return new MessageLog("Erro durante a execução do processo!"); } }
    }
}
