// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$("#run-test-button").on("click", gogopowerrangers);

if (window.location.pathname.lastIndexOf("/runTest") === 0) {
    loadRunTestEventHandlers()
}

function loadRunTestEventHandlers() {
    $("#check").on("click", check);
    $("#next").on("click", next);
    $("#reset").on("click", reset);
}

function check() {
    $(".question")
        .not(".d-none")
        .find(".answer-text")
        .each(function (index) {
            let e = $(this);

            if (e.data("correct") === "True") {
                e.addClass("text-success")
            } else {
                e.addClass("text-danger")
            }
        })
}

function next() {
    let visible = $(".question")
        .not(".d-none");

    let invisibleUncompleted = $(".question.d-none")
        .not(".completed");

    let nextGroup = invisibleUncompleted.slice(0, questionCount);

    visible.addClass("d-none completed");
    nextGroup.removeClass("d-none");

    if (invisibleUncompleted.length === 0) {
        $("#next").addClass("d-none");
        $("#reset").removeClass("d-none");        
    }
}

function reset() {
    location.reload();
}

function gogopowerrangers() {
    $(this).closest("form").submit();    
}
