namespace Dapper.Practice.UserAnonimizer
{
    using System.Linq;
    using System.Threading.Tasks;
    using Dapper.Practice.DAL.Entity;
    using Dapper.Practice.DAL.Repository;

    public class PersonalDataCleaner
    {
        private string email;
        private string alias;
        private User userToHide;

        public PersonalDataCleaner(string email, string alias)
        {
            this.email = email;
            this.alias = alias;
        }

        public async void RunAsync()
        {
            await this.ReplaceUserData();
            await this.ReplaceMailData();
            System.Console.WriteLine("Removed!");
        }

        private async Task ReplaceUserData()
        {
            var users = new DapperRepository<User>("Users");
            this.userToHide = (await users.Find(x => x.Email == this.email)).FirstOrDefault();

            if (this.userToHide == null)
            {
                return;
            }

            users.Update(new User()
            {
                Id = this.userToHide.Id,
                Name = this.alias,
                Surname = this.alias,
                Email = this.alias,
                DateOfBirth = this.userToHide.DateOfBirth,
            });
        }

        private async Task ReplaceMailData()
        {
            var mails = new DapperRepository<Mail>("Mails");
            var mailsCollection = await mails.GetAll();
            var contentProps = typeof(MailContent).GetProperties();

            foreach (var mail in mailsCollection)
            {
                var content = MailContentConverter.FromJSON(mail.Object);
                var isContainPersonalData = false;

                foreach (var contentProp in contentProps)
                {
                    var contentPropValue = contentProp.GetValue(content).ToString();

                    if (contentPropValue.Contains(this.email))
                    {
                        contentProp.SetValue(content, contentPropValue.Replace(this.email, this.alias));
                        isContainPersonalData = true;
                        contentPropValue = contentProp.GetValue(content).ToString();
                    }

                    if (this.userToHide != null && contentPropValue.Contains(this.userToHide.Surname))
                    {
                        contentProp.SetValue(content, contentPropValue.Replace(this.userToHide.Surname, this.alias));
                        isContainPersonalData = true;
                        contentPropValue = contentProp.GetValue(content).ToString();
                    }

                    if (this.userToHide != null && contentPropValue.Contains(this.userToHide.Name))
                    {
                        contentProp.SetValue(content, contentPropValue.Replace(this.userToHide.Name, this.alias));
                        isContainPersonalData = true;
                    }
                }

                if (isContainPersonalData)
                {
                    mail.Object = MailContentConverter.ToJSON(content);
                    mails.Update(mail);
                }
            }
        }
    }
}
