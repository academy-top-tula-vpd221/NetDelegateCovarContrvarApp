Message message = new EmailMessage("Hello");

// ковариантность - про полиморфизм возвращаемого значения
MessageCreate messageCreate;
messageCreate = EmailMessageCreate;
EmailMessage EmailMessageCreate(string text)
{
    return new EmailMessage(text);
}


// контрвариантность - про полиморфизм входных аргументов
EmailReceiver emailReceiver;
emailReceiver = ReceiverMessage;
void ReceiverMessage(Message message)
{
    message.Print();
}

// ковариантность обобщенного делегата
MessageBuilder<Message> messageBuilder;
messageBuilder = EmailMessageBuilder;
EmailMessage EmailMessageBuilder(string text)
{
    return new EmailMessage(text);
}

// контрвариантность обобщеного делегата
MessageReceiver<Message> messageReceiver = (Message message) => message.Print();
MessageReceiver<EmailMessage> emailMessReceiver = messageReceiver;

messageReceiver(new Message("Hello world"));
messageReceiver(new EmailMessage("Hello people"));


// совмещение
MessageConverter<Message, EmailMessage> toEmailConverter =
   //(Message message) => new EmailMessage(message.Text);
   ToEmailConverter;

MessageConverter<SmsMessage, Message> toMessageConverter = toEmailConverter;
Message message1 = toMessageConverter(new SmsMessage("Hello sms message"));
message1.Print();


EmailMessage ToEmailConverter(Message message)
{
    return new EmailMessage(message.Text);
}

class Message
{
    public string Text { set; get; }
    public Message(string text) => Text = text;
    public virtual void Print() => Console.WriteLine(Text);
}

class EmailMessage : Message
{
    public EmailMessage(string text) : base(text) { }
    public override void Print() => Console.WriteLine($"Email message: {Text}");
}

class SmsMessage : Message
{
    public SmsMessage(string text) : base(text) { }
    public override void Print() => Console.WriteLine($"Sms message: {Text}");
}

delegate Message MessageCreate(string message);

delegate void EmailReceiver(EmailMessage message);

delegate T MessageBuilder<out T>(string message);

delegate void MessageReceiver<in T>(T message);

delegate R MessageConverter<in T, out R>(T message);