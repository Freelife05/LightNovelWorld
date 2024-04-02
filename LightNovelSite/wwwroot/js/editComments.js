$(document).ready(function () {

    $(document).on("click", ".edit-comment", function (e) {
        e.preventDefault();
        var commentContent = $(this).parent().find(".comment-content");
        commentContent.attr("contenteditable", "true");
        commentContent.focus();
        $(this).parent().find(".edit-comment").hide();
        $(this).parent().find(".edit-comment-box").val(commentContent.text()); // Set initial value for textarea
        $(this).parent().find("form").show();
    });

    $(document).on("click", ".cancel-edit", function (e) {
        e.preventDefault();
        var commentContent = $(this).closest(".comment").find(".comment-content");
        commentContent.attr("contenteditable", "false");
        $(this).closest(".comment").find(".edit-comment").show();
        $(this).closest(".comment").find("form").hide();
    });
});
