// See https://aka.ms/new-console-template for more information
using Azure.Communication.Email;
using Azure.Communication.Email.Models;

Console.WriteLine("Hello, World!");

var connectionString = @"endpoint=";

var emailClient = new EmailClient(connectionString);
// EmailContentの文字列引数はメールタイトル
EmailContent emailContent = new EmailContent("Welcome to Azure Communication Service Email APIs.");

// 内容(Htmlが設定されたらHTML側が優先される？)
emailContent.PlainText = "This email meessage is sent from Azure Communication Service Email using .NET SDK.";
emailContent.Html = "<h1>HTMLのあれやこれや</h1>";

// 送信するアドレスの設定
List<EmailAddress> emailAddresses = new List<EmailAddress> { new EmailAddress("t_o_19880522@outlook.com") };
EmailRecipients emailRecipients = new EmailRecipients(emailAddresses);

EmailMessage emailMessage = new EmailMessage("devtakas@8038d09c-272f-40ea-a37e-2b1c9b16fc4c.azurecomm.net", emailContent, emailRecipients);

// 添付ファイル
string base64St = Convert.ToBase64String(File.ReadAllBytes(@"C:\temp\sample.txt"));
EmailAttachment attachment =  new EmailAttachment("さんぷる.txt", EmailAttachmentType.Txt, base64St);
emailMessage.Attachments.Add(attachment);

SendEmailResult emailResult = emailClient.Send(emailMessage, CancellationToken.None);


// 送信結果の確認
var status = emailClient.GetSendStatus(emailResult.MessageId);

Console.WriteLine("おわり" + status.Value);