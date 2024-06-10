let intervalId;
let globalLmTimestamp = "_novalue_";
let conversationID = null;
let NMessagesAvailable = false;

let loadMessagesOnScroll = (conversationId, OMTimestamp) => {

    $.ajax({
        url: `/api/messages/getmessages/${conversationId}/${OMTimestamp}`,
        type: "GET",
        success: function (messages) {
            // Handle the API response here

            $.each(messages, (i) => {

                if (messages[i].sender == userId) {
                    let newHtml = `<div class="sent-msg" data-timestamp="${messages[i].mTimestamp}">
                                        <p>
                                            ${messages[i].msg}
                                        </p>
                                   </div>`

                    $(".conversation").html((i, oldHtml) => {
                        return newHtml + oldHtml;
                    })
                } else {
                    let newHtml = `<div class="received-msg" data-timestamp="${messages[i].mTimestamp}">
                                        <p>
                                            ${messages[i].msg}
                                        </p>
                                   </div>`

                    $(".conversation").html((i, oldHtml) => {
                        return newHtml + oldHtml;
                    })
                }
            })
        },
        error: function (xhr, status, error) {
            // Handle errors here
            console.error(status, error);
        }
    })
}

let listenToScroll = () => {
    let conversation = document.querySelector(".conversation");

    setTimeout(() => {
        conversation.scrollTop = conversation.scrollHeight;
    }, 100)

    $(".conversation").scroll((e) => {
        if (conversation.scrollTop == 0) {
            oldestMTimestamp = conversation.childNodes[0].dataset.timestamp;
            loadMessagesOnScroll(conversationID, oldestMTimestamp);
        }
    })
}

//Gets the Last Message Timestamp from dataset after initial load
let getLMTimestamp = () => {
    let conversation = document.querySelector(".conversation");
    let convLength = conversation.childNodes.length;
    if (convLength > 1) {
        globalLmTimestamp = conversation.childNodes[convLength - 2].dataset.timestamp;
    }

    return globalLmTimestamp;
}

//Adds message to the database
let addMessage = (conversationId, senderId, receiverId, msgString) => {
    let message = {
        ConversationID: conversationId,
        Sender: senderId,
        Receiver: receiverId,
        Msg: msgString,
    }

    $.ajax({
        url: "/api/messages/postmessage",
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(message),
        success: function (response) {
            // Handle the API response here
            getConversations();
        },
        error: function (xhr, status, error) {
            // Handle errors here
            console.error(status, error);
        }
    });
}

//Loads initial set of messages after conversation opens
let loadMessages = (conversationId) => {
    conversationID = conversationId;

    let MessageTS = "_novalue_";;
    globalLmTimestamp = "_novalue_";

    $.ajax({
        url: `/api/messages/getmessages/${conversationId}/${MessageTS}`,
        type: "GET",
        success: function (messages) {
            // Handle the API response here
            
            $.each(messages, (i) => {

                if (messages[i].sender == userId) {
                    let newHtml = `<div class="sent-msg" data-timestamp="${messages[i].mTimestamp}">
                                        <p>
                                            ${messages[i].msg}
                                        </p>
                                   </div>`

                    $(".conversation").html((i, oldHtml) => {
                        return newHtml + oldHtml;
                    })
                } else
                {
                    let newHtml = `<div class="received-msg" data-timestamp="${messages[i].mTimestamp}">
                                        <p>
                                            ${messages[i].msg}
                                        </p>
                                   </div>`

                    $(".conversation").html((i, oldHtml) => {
                        return newHtml + oldHtml;
                    })
                }
            })

            globalLmTimestamp = getLMTimestamp();
            listenToScroll();
        },
        error: function (xhr, status, error) {
            // Handle errors here
            console.error(status, error);
        }
    })
    
}

//loads new massages based on the latest message timestamp
let loadNewMessages = async (conversationId, lmTimestamp) => {
    if (intervalId) {
        clearInterval(intervalId);
    }
    NMessagesAvailable = false;

    $.ajax({
        url: `/api/messages/getnewmessages/${conversationId}/${lmTimestamp}`,
        type: "GET",
        success: function (newMessages) {
            // Handle the API response here
            $.each(newMessages, (i) => {

                if (newMessages[i].sender == userId) {
                    let newHtml = `<div class="sent-msg" data-timestamp="${newMessages[i].mTimestamp}">
                                        <p>
                                            ${newMessages[i].msg}
                                        </p>
                                   </div>`

                    $(".conversation").html((i, oldHtml) => {
                        return oldHtml + newHtml;
                    })
                } else {
                    let newHtml = `<div class="received-msg" data-timestamp="${newMessages[i].mTimestamp}">
                                        <p>
                                            ${newMessages[i].msg}
                                        </p>
                                   </div>`

                    $(".conversation").html((i, oldHtml) => {
                        return oldHtml + newHtml;
                    })
                }
                // changing global timestamp for last message which is different the local lmTimestamp
                globalLmTimestamp = newMessages[i].mTimestamp;
                
            })
        },
        error: function (xhr, status, error) {
            // Handle errors here
            console.error(status, error);
        }
    })

}

//checks for new messages in conversation
let checkNewMessages = async (conversationId) => {

    $.ajax({
        url: `/api/messages/checkmessages/${conversationId}/${globalLmTimestamp}`,
        type: "GET",
        success: function (response) {
            // Handle the API response here
            if (response > 0) {
                NMessagesAvailable = true;
            }
        },
        error: function (xhr, status, error) {
            // Handle errors here
            console.error(status, error);
        }
    })
}


//A polling function that constantly calls the check and load functions
(function startInterval() {
    intervalId = setInterval(async () => {
        if (intervalId) {
            clearInterval(intervalId);
        }

        if (conversationID != null) {
            await checkNewMessages(conversationID);

            if (NMessagesAvailable == true) {

                await loadNewMessages(conversationID, globalLmTimestamp);
                let conversation = document.querySelector(".conversation");
                conversation.scrollTop = conversation.scrollHeight;
            }
        }

        startInterval();
    }, 1000)
})();

$(document).on('click', '.msg-btn', (e) => {
    let connIco = document.querySelector(".conn-ico");
    let conversationId = `${connIco.dataset.conversationId}`;
    let senderId = document.querySelector(".self-cont").dataset.userId;
    let receiverId = document.querySelector(".user-cont").dataset.userId;
    let msgString = document.querySelector(".msg-input").value;

    addMessage(conversationId, senderId, receiverId, msgString);

    document.querySelector(".msg-input").value = "";
    e.preventDefault();
});

//$(".conversation").scroll((e) => {
//    let conversation = document.querySelector(".conversation");
//    console.log("conversation.scrollHeight")
//})

//let conversation = document.querySelector(".conversation");
//conversation.addEventListener('scroll', function (event) {
//    console.log('scrolling', event.target);
//}, true );

