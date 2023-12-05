mergeInto(LibraryManager.library, {

	GiveMePlayerData: function () {
    	myGameInstance.SendMessage('Yandex', 'SetName', player.getName());
    	myGameInstance.SendMessage('Yandex', 'SetPhoto', player.getPhoto("medium"));
      console.log(player.getName());
  	},

  	RateGame: function () {
    
    	ysdk.feedback.canReview()
        .then(({ value, reason }) => {
            if (value) {
                ysdk.feedback.requestReview()
                    .then(({ feedbackSent }) => {
                        console.log(feedbackSent);
                    })
            } else {
                console.log(reason)
            }
        })
  	},

    AuthorizationYG: function () {
            ysdk.getPlayer({ scopes: false }).then(_player => {      
                player = _player;

                if (_player.getMode() === 'lite') {

                }
                else{
                    myGameInstance.SendMessage('Yandex', 'HideAuthorizationYandexButtonAndLoadData');
                }
            }).catch(err => {
                console.log("Auth1 failed");
            });
    },

    OpenAuthDialogYG: function () {
       ysdk.auth.openAuthDialog().then(() => {
            
             // Èãðîê óñïåøíî àâòîðèçîâàí
             console.log("Autorization succeeded");            
             myGameInstance.SendMessage('Yandex', 'HideAuthorizationYandexButtonAndLoadData');
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
        myGameInstance.SendMessage('DataLoader', 'CompareDataFromTheDeviceAndFromTheCloud', myJSON);
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
             myGameInstance.SendMessage('Ads', 'OnInterstitialAdsClosed');
          },
          onError: function(error) {
             myGameInstance.SendMessage('Ads', 'OnInterstitialAdsClosed');
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
          myGameInstance.SendMessage('Ads', 'OnAdsReceivedReward');
          },
          onClose: () => {
          console.log('Video ad closed.');
          myGameInstance.SendMessage('Ads', 'OnRewardedAdsClosed');
          }, 
          onError: (e) => {
          myGameInstance.SendMessage('Ads', 'OnRewardedAdsEror');
          }
        }
      })
    },

  });