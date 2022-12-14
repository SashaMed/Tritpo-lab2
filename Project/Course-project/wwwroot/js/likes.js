var _userId = $('userId').val();
var _itemId = $("#itemId").val() - 0;
var _return;
var _checkboxes = document.getElementById('toggle-heart');


$.ajax({
    url: "/CommentsLikes/GetLikesCount",
    data: { id: _itemId },
    dataType: "json",
    type: 'GET',
    success: function (data) {
        console.log("Success!" + data);
        _return = data;
        $("#likes").text(data);
    },
    error: function () { console.log('error-GetLikesCount1'); }
});


$.ajax({
    url: "/CommentsLikes/GetLikesCheck",
    data: { id: _itemId },
    dataType: "json",
    type: 'GET',
    success: function (data) {
        console.log("Success!" + data);
        console.log(data - 0)
        if (data - 0 == 1) {
            _checkboxes.checked = true;
        }
    },
    error: function () { console.log('error-GetLikesCheck'); }
});




$("#toggle-heart").click(function () {
    $.ajax({
        url: "/CommentsLikes/OnLikeClick",
        data: { id: _itemId },
        dataType: "json",
        type: 'GET',
        success: function (data) {
            console.log("Success!" + data);
            _return = data;
            $("#likes").text(data);
        },
        error: function () { console.log('error-GetLikesCount1'); }
    });


});