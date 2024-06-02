<h1>Educational Management System</h1>
<h3>Hi, I am student of programming college, this is my first project using the ASP.NET Core</h3>
<p>
  In the master branch there are two projects (Developer version and Release version).
  <ul>
    <li>If you want to change something etc, then use <b>Developer version</b>, that contains scss files and other things that you need.</li>
    <li>If you want run this project using Docker, then use Release version.</li>
  </ul>
  <h4>Preparation:</h4>
  <p>
    To run this project you need to have:
    <ul>
      <li>Visual Studio IDE or Visual Studio code editor;</li>
      <li>Installed .NET SDK version 8.0;</li>
      <li>Installed ASP.NET Core Web API and ASP.NET Core MVC;</li>
      <li>MySQL server (local or remoted) version 7.0+;</li>
      <li>Stable internet connection.</li>
    </ul>
  </p>
  <h3>1) Developer version</h3>
  <p>
    To start the project, follow the following steps:</br>
    1.1 Copy Developer version from github using zip file or ssh connection string;</br>
    1.2 Install MySQL or using server with installed one;</br>
    1.3 Authorize on the MySQL server;</br>
    1.4 Run these scripts:</br>
      <code>CREATE DATABASE edSystemDB;</code></br>
      <code>USE edSystemDB;</code></br>
    1.5 Open <i>edSystemDB.sql</i> file, copy everything from it;</br>
    1.6 Paste in the query command line in the MySQL;</br>
    1.7 Get back to IDE (editor) and open the <code>EdManagementSystem.App</code> folder;</br>
    1.8 Open <code>appsettings.json</code> file and change this line <code>server=ip_address;database=db_name;uid=login;password=password;</code> using your data;</br>
    1.9 Do the same thing with the <code>EdManagementSystem.API</code> folder and change this email configuration for password recovery: </br>
    <code>  "EmailConfiguration": {
    "SmtpServer": "smtp.mail.ru",
    "SmtpPort": 587,
    "SmtpUsername": "your_email",
    "SmtpPassword": "your_password"
  }</code>;</br><b>Note:</b> smptPassword is not your email password, you have to generate some password for application using your email site. For example for mail.ru follow 
    <a href="https://help.mail.ru/mail/security/protection/external/">this site</a> to generate this password</br>
    2.0 That's it, then just use <code>dotnet build</code> <code>dotnet run</code> commands to run this project.
  </p>
    <h3>2) Release version</h3>
  <p>
    To start the project, follow the following steps:</br>
    1.1 Copy Developer version from github using zip file or ssh connection string;</br>
    1.2 Install MySQL or using server with installed one;</br>
    1.3 Authorize on the MySQL server;</br>
    1.4 Run these scripts:</br>
      <code>CREATE DATABASE edSystemDB;</code></br>
      <code>USE edSystemDB;</code></br>
    1.5 Open <i>edSystemDB.sql</i> file, copy everything from it;</br>
    1.6 Paste in the query command line in the MySQL;</br>
    1.7 Get back to IDE (editor) and open the <code>EdManagementSystem.App</code> folder;</br>
    1.8 Open <code>appsettings.json</code> file and change this line <code>server=ip_address;database=db_name;uid=login;password=password;</code> using your data;</br>
    1.9 Do the same thing with the <code>EdManagementSystem.API</code> folder and change this email configuration for password recovery: </br>
    <code>  "EmailConfiguration": {
    "SmtpServer": "smtp.mail.ru",
    "SmtpPort": 587,
    "SmtpUsername": "your_email",
    "SmtpPassword": "your_password"
  }</code>;</br><b>Note:</b> smptPassword is not your email password, you have to generate some password for application using your email site. For example for mail.ru follow 
    <a href="https://help.mail.ru/mail/security/protection/external/">this site</a> to generate this password</br>
    2.0 Go to main folder of project and run command <code>docker compose up -d</code>.
  </p>

  <h4>Use these credentials to authorize and test project: </h4>
  <span><b>Email</b>: blabla@yandex.ru</span></br>
  <span><b>Password</b>: Plazma_2004_2020</span>
  <h4>Thanks for following, if there will be some questions, you can write me here: <a href="https://t.me/nikoyanErik">Telegram</a></h4>
</p>
