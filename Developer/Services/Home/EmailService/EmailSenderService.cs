﻿using System;
using System.Configuration;
using System.IO;
using Developer.Models;
using Developer.Models.ApplicationModels;
using Developer.Models.ViewModels;
using RazorEngine.Templating;

namespace Developer.Services.Home
{
    public class EmailSenderService : IEmailSenderService
    {
        private readonly ITemplateService _templateSercvie;
        private readonly ISmtpManager _smtpManager;
        private readonly string _domainName;

        public EmailSenderService(ITemplateService templateSercvie, ISmtpManager smtpManager)
        {
            _templateSercvie = templateSercvie;
            _smtpManager = smtpManager;
            _domainName = ConfigurationManager.AppSettings["emailDomain"];
        }

        public Result SendOfferQuestion(ContactEmail contactEmail)
        {
            var body = _templateSercvie.Parse(GetOfferTemplate(), contactEmail, null, null);

            var emailMsg = new EmailMessage()
            {
                Body = body,
                Destination = _domainName,
                Topic = "Pytanie od użytkownika dotyczące oferty"
            };

            _smtpManager.SendEmail(emailMsg);

            return new Result(true, null ,"");
        }

        public Result SendContactQuestion(ContactEmail contactEmail)
        {
            var body = _templateSercvie.Parse(GetContactTemplate(), contactEmail, null, null);

            var emailMsg = new EmailMessage()
            {
                Body = body,
                Destination = _domainName,
                Topic = "Pytanie od użytkownika"
            };

            _smtpManager.SendEmail(emailMsg);

            return new Result(true, null, "");
        }

        public Result SendOfferResponse(OfferStatus offerStatus, string destination)
        {
            string template;

            if (offerStatus == OfferStatus.Accepted)
            {
                template = GetOfferAcceptedTemplate();
            }
            else
            {
                template = GetOfferRejectedTemplate();
            }

            var body = _templateSercvie.Parse(template, null, null, null);

            var emailMsg = new EmailMessage()
            {
                Body = body,
                Destination = destination,
                Topic = "Odpowiedź dotycząca oferty"
            };

            _smtpManager.SendEmail(emailMsg);

            return new Result(true, null, "");
        }


        public string GetOfferRejectedTemplate()
        {
            string template =
                File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                    "Services/Home/EmailService/Templates/OfferRejectedEmailTemplate.cshtml"));
            return template;
        }

        public string GetOfferAcceptedTemplate()
        {
            string template =
                File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                    "Services/Home/EmailService/Templates/OfferAcceptedEmailTemplate.cshtml"));

            return template;
        }

        public string GetOfferTemplate()
        {
            string template = File.ReadAllText(
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                    "Services/Home/EmailService/Templates/QuestionEmailTemplate.cshtml"));

            return template;
        }

        public string GetContactTemplate()
        {
            string template = File.ReadAllText(
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                    "Services/Home/EmailService/Templates/ContactEmailTemplate.cshtml"));

            return template;
        }
    }
}