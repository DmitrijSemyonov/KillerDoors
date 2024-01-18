mergeInto(LibraryManager.library, {

    AuthorizationYG: function () {
            ysdk.getPlayer({ scopes: false }).then(_player => {      
                player = _player;

                if (_player.getMode() === 'lite') {

                }
                else{
                    myGameInstance.SendMessage('Yandex', 'InformAuthorized');
                }
            }).catch(err => {
                console.log("Auth1 failed");
            });
    },

    OpenAuthDialogYG: function () {
       ysdk.auth.openAuthDialog().then(() => {
            
             // Èãðîê óñïåøíî àâòîðèçîâàí
             console.log("Autorization succeeded");            
             myGameInstance.SendMessage('Yandex', 'InformAuthorized');
             AuthorizationYG();
        }).catch(() => {

             console.log("Autorization failed");
             // Èãðîê íå àâòîðèçîâàí.

        });
    },

    SaveExtern: function (data) {
      var dataString = UTF8ToString(data);
      var object = JSON.parse(dataString);
      player.setData(object).then(() => {
        console.log('data is set');
      });
    },

    LoadExtern: function () {
      player.getData().then(_data => {
        const myJSON = JSON.stringify(_data);
        myGameInstance.SendMessage('Yandex', 'SendDataFromTheCloud', myJSON);
     // })
        }).catch(err => {
        // Îøèáêà ïðè èíèöèàëèçàöèè îáúåêòà Player.
        });
    },
    
    GetLanguage: function () {
      var language = ysdk.environment.browser.lang;
      var bufferSize = lengthBytesUTF8(language) + 1;
      var buffer = _malloc(bufferSize);
      stringToUTF8(language, buffer, bufferSize);
      return buffer;
    },
    GetLanguageI18N: function () {
      var language = ysdk.environment.i18n.lang;
      var bufferSize = lengthBytesUTF8(language) + 1;
      var buffer = _malloc(bufferSize);
      stringToUTF8(language, buffer, bufferSize);
      return buffer;
    },

    ShowInterstitial: function () {
      ysdk.adv.showFullscreenAdv({
        callbacks: {
          onClose: function(wasShown) {
             myGameInstance.SendMessage('Yandex', 'OnInterstitialAdsClosed');
          },
          onError: function(error) {
             myGameInstance.SendMessage('Yandex', 'OnInterstitialAdsClosed');
          }
        }
      })
    },

    ShowRewarded: function () {
      ysdk.adv.showRewardedVideo({
        callbacks: {
          onOpen: () => {
            console.log('Video ad open.');
          },
          onRewarded: () => {
            console.log('Rewarded!');
            myGameInstance.SendMessage('Yandex', 'OnAdsReceivedReward');
          },
          onClose: () => {
            console.log('Video ad closed.');
            myGameInstance.SendMessage('Yandex', 'OnRewardedAdsClosed');
          }, 
          onError: (e) => {
            myGameInstance.SendMessage('Yandex', 'OnRewardedAdsEror');
          }
        }
      })
    },

  });