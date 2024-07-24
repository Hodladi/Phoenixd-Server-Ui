# Phoenixd Server UI

You can either self-host it with this repo or you can try use it through [pWallet.app](https://pwallet.app) by adding your phoenixd server url and password in settings.
If you self-host it you can chose to add your backend settings (baseUrl for phoneix server and also its password) in appsettings.json or you can use settings button in UI to save to local storage.
The settings is only saved in local storage on your device.

This UI with the help of Acinq Phoenix Server can handle the following:

**Scan QR**
- BOLT11
- BOLT12
- LNURL
- Lightning Address
- LNURL Auth

**Create invoice**
- BOLT11 invoice with amount
- BOLT12 invoice without amount

**Pay invoice**
- BOLT11
- BOLT12
- Lightning address (BIP-353 or LNURL)
- LNURL

I probably missed some functionality but what Phoenix Server can handle, this UI can handle.
The only thing missing is **send to bitcoin address** (on-chain). That is ment to be implemented but i prioritized the lightning functionality.
  
<hr/>
When new code is pushed to this repo i automatically builds a new docker image.
This is avalible here.

[Docker hub, Phoenixd-Server-Ui](https://hub.docker.com/r/hodladi21/phoenixd-server-ui)

```git clone https://github.com/Hodladi/Phoenixd-Server-Ui.git```

```cd Phoenixd-Server-Ui```

```docker build -t pwallet .```

```docker run -d -p 2291:2291 --name pwallet_container pwallet```

<p>The application is running on port 2291 inside the container. If you want to change that please edit Program.cs and appsettings.json before you build.</p>
<p></p>

<hr/>

<img src="https://nostrich.cc/github/1.jpg" width="150px"/> <img src="https://nostrich.cc/github/2.jpg" width="150px"/>

<img src="https://nostrich.cc/github/3.jpg" width="150px"/> <img src="https://nostrich.cc/github/4.jpg" width="150px"/> <img src="https://nostrich.cc/github/5.jpg" width="150px"/>

<img src="https://nostrich.cc/github/6.jpg" width="150px"/> <img src="https://nostrich.cc/github/7.jpg" width="150px"/> <img src="https://nostrich.cc/github/8.jpg" width="150px"/>

<img src="https://nostrich.cc/github/9.jpg" width="150px"/>

<img src="https://nostrich.cc/github/10.jpg" width="150px"/> <img src="https://nostrich.cc/github/11.jpg" width="150px"/>




