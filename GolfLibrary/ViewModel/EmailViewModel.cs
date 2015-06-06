using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GolfClub.Model;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Windows.Input;

namespace GolfClub.ViewModel
{
    public class EmailViewModel : ViewModelBase
    {
        #region Fields

        private readonly IWindowService _windowService;
        private string _attachment;
        private bool _dummy;
        private string _attachmentButtonText;

        #endregion Fields

        #region Constructors

        public EmailViewModel(IWindowService windowService)
        {
            _windowService = windowService;

            SendCommand = new RelayCommand(SendMail);

            AttachCommand = new RelayCommand(AddAttachment);

            Attachment = string.Empty;

            AttachmentButtonText = "Attach";
        }

        #endregion Constructors

        #region Properties

        public string Body { get; set; }

        public List<string> EmailAddresses { get; set; }

        public string FromAddress { get; set; }

        public string Password { get; set; }

        public ICommand SendCommand { get; set; }

        public string Smtp { get; set; }

        public string Subject { get; set; }

        public string User { get; set; }

        public ICommand AttachCommand { get; set; }

        public Visibility HasAttachment
        {
            get
            {
                return string.IsNullOrWhiteSpace(Attachment) ? Visibility.Hidden : Visibility.Visible;
            }
        }

        public string Attachment
        {
            get { return _attachment; }
            set { if (Set("Attachment", ref _attachment, value)) Set("HasAttachment", ref _dummy, !_dummy); }
        }

        public string AttachmentButtonText
        {
            get { return _attachmentButtonText; }
            set { Set("AttachmentButtonText", ref _attachmentButtonText, value); }
        }

        #endregion Properties

        #region Methods

        private void SendMail()
        {
            using (var mail = new MailMessage())
            {
                mail.From = new MailAddress(FromAddress);
                foreach (var emailAddress in EmailAddresses)
                {
                    mail.To.Add(new MailAddress(emailAddress));
                }
                mail.Subject = Subject;
                mail.Body = Body;
                mail.IsBodyHtml = false;
                if (!string.IsNullOrWhiteSpace(Attachment))
                {
                    mail.Attachments.Add(new Attachment(Attachment));
                }
                {
                    
                }
                using (var smtp = new SmtpClient(Smtp, 587))
                {
                    smtp.Credentials = new NetworkCredential(FromAddress, Password);
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }
        }

        private void AddAttachment()
        {
            if (string.IsNullOrWhiteSpace(Attachment))
            {
                Attachment = _windowService.GetAttachment();
                if (!string.IsNullOrWhiteSpace(Attachment))
                {
                    AttachmentButtonText = "Detach";
                }
            }
            else
            {
                Attachment = string.Empty;
                AttachmentButtonText = "Attach";
            }
        }

        #endregion Methods
    }
}