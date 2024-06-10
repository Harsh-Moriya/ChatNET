let openConversation = (conversationid) => {
    $.ajax({
        url: "/Home/_ConversationPanel",
        type: "POST",
        data: { "conversationId": conversationid },
        success: function (response) {
            document.querySelector(".conversation-panel").innerHTML = response;
            loadMessages(conversationid)
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });
}

let createAndOpenConversation = (usertwoid) => {
    let users = {
        uone: userId,
        utwo: usertwoid,
    }

    $.ajax({
        url: "/api/conversations/postconversation",
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(users),
        success: function (response) {
            // Handle the API response here
            if (response.success) {
                openConversation(response.conversationid);
            }
        },
        error: function (xhr, status, error) {
            // Handle errors here
            console.error(status, error);
        }
    });
}

$(document).on('dblclick', '#conversation-user', (e) => {
    let conversationID = `${e.currentTarget.dataset.conversationId}`
    openConversation(conversationID);
    e.preventDefault();
});

$(document).on('dblclick', '#searched-user', (e) => {
    let usertwo = e.currentTarget.querySelector("h6").textContent;
    createAndOpenConversation(usertwo);
    e.preventDefault();
});