//Global Page counter
let currentPage = 0;
let userId = document.querySelector("#nav-userid").textContent;

// Gets the user conversations from the conversation table
let getConversations = () => {
    currentPage = 0;
    $.ajax({
        url: `/api/conversations/getconversations/${userId}`,
        method: 'GET',
        success: function (conversations) {
            // Handle the API response here
            $(".users").html(() => {
                return "";
            });

            $.each(conversations, (i) => {
                let initial = "";
                let userName = "";
                let userID = "";

                if (`${userId}` == conversations[i].userOne) {
                    initial = conversations[i].userTwoName.charAt(0);
                    userName = conversations[i].userTwoName;
                    userID = conversations[i].userTwo;
                }
                if (`${userId}` == conversations[i].userTwo) {
                    initial = conversations[i].userOneName.charAt(0);
                    userName = conversations[i].userOneName;
                    userID = conversations[i].userOne;
                }

                let newHTML = `<div class="user" id="conversation-user">
                                        <div class="user-initial">
                                            <span>${initial}</span>
                                        </div>
                                        <div class="user-name">
                                            <h5>${userName}</h5>
                                            <h6>${userID}</h6>
                                        </div>
                                       </div>`

                $(".users").html((i, oldHTML) => {
                    return oldHTML + newHTML;
                })
            })
        },
        error: function (xhr, status, error) {
            // Handle errors here
            console.error(status, error);
        }
    });
}

//fetches users from the users table
let getUsers = (value) => {
    $.ajax({
        url: `/api/users/getusers/${userId}/${value}/${currentPage}`,
        method: 'GET',
        success: function (users) {
            // Handle the API response here
            if (currentPage == 1) {
                $(".users").html(() => {
                    return "";
                });
            }

            if (value == "_novalue_") {
                getConversations();
            } else {
                $.each(users, (i) => {
                    let initial = users[i].name.charAt(0);
                    let userName = users[i].name;
                    let userID = users[i].userId

                    let newHTML = `<div class="user" id="searched-user">
                                                        <div class="user-initial">
                                                            <span>${initial}</span>
                                                        </div>
                                                        <div class="user-name">
                                                            <h5>${userName}</h5>
                                                            <h6>${userID}</h6>
                                                        </div>
                                                       </div>`

                    $(".users").html((i, oldHTML) => {
                        return oldHTML + newHTML;
                    })
                })
            }
        },
        error: function (xhr, status, error) {
            // Handle errors here
            console.error(status, error);
        }
    });
}

//fetches users from the users table based on input
let getUsersOnChange = () => {
    let value = $(".form-input").val();
    if (value == "") {
        value = "_novalue_"
    }

    currentPage = 1;

    getUsers(value);
}

let getUsersOnScroll = () => {
    if (currentPage >= 1 && currentPage < 3) {
        let value = $(".form-input").val();
        if (value == "") {
            value = "_novalue_"
        }

        currentPage++;

        getUsers(value);
    }
}

$(".users").scroll(() => {
    let users = document.querySelector(".users");
    var scrollY = users.scrollHeight - users.scrollTop;
    var height = users.offsetHeight;
    var offset = height - scrollY;

    if (offset == 0 || offset == 1) {
        setTimeout(function () {
            getUsersOnScroll();
        }, 500)
    }
});

$(".users").ready(getConversations);

$(".form-input").change(getUsersOnChange);