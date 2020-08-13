namespace boticario.Helpers.Enums
{
    public class MessageError
    {
        public string Value { get; set; }

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
        public string Value { get; set; }

        private MessageSuccess(string value) { Value = value; }

        public static MessageSuccess Update { get { return new MessageSuccess("Registro atualizado com sucesso!"); } }
        public static MessageSuccess Delete { get { return new MessageSuccess("Registro excluído com sucesso!"); } }
    }
}
