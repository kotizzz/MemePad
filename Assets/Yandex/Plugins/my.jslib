mergeInto(LibraryManager.library, {

	Hello: function(){
		window.alert("Hello world!");
		console.log("Hello world!");
	},

	GiveMePlayerData: function(){
		myGameInstance.SendMessage('Yandex', 'SetName', player.getName());
		myGameInstance.SendMessage('Yandex', 'SetPhoto', player.getPhoto());

		console.log(player.getName());
		console.log(player.getPhoto("medium"));
	},

	RateGame: function(){
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

});