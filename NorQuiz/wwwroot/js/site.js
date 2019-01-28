// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$("#run-test-button").on("click", gogopowerrangers);

if (window.location.pathname.lastIndexOf("/RunTest") === 0) {
    loadRunTestEventHandlers()
}

function loadRunTestEventHandlers() {
    $("#check").on("click", check);
    $("#next").on("click", next);
    $("#reset").on("click", reset);
}

function check() {
    let activeAnswers = $(".question")
        .not(".d-none")
        .not(".completed")
        .find(".answer");
    
    activeAnswers
        .each(function (e) {
            let answer = $(activeAnswers[e]);
            let text = answer.find(".answer-text");
            if (answer.data("correct") === "True") {
                text.addClass("text-success")
            } else {
                text.addClass("text-danger")
            }

            let checkbox = answer.find(".answer-checkbox");
            checkbox.prop("disabled","disabled");
            if ((!checkbox.is(':checked') && answer.data("correct") === "True" ||
                checkbox.is(':checked') && answer.data("correct") === "False")
            && !answer.closest(".question").hasClass("incorrect")) {                
                $("#quiz").data().errors++;
                answer.closest(".question").addClass("incorrect").addClass("text-danger")
                
                
                
            }
        });

    $(this).addClass("d-none");
    $("#next").removeClass("d-none");
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
        $("#check").addClass("d-none");
        $("#reset").removeClass("d-none");
        showResults();
    }
    else{
        $("#check").removeClass("d-none"); 
    }
    
    $(this).addClass("d-none");    
}

function showResults() {
    let quiz = $("#quiz").addClass("d-none");
    
    let results = $("#results").removeClass("d-none");
    
    let errorText = results.find("#error-text");
    if (quiz.data("errors") > 0){
        errorText
            .text("Ошибок: "+ quiz.data("errors"))
            .addClass("text-danger");
        results
            .find("#fail-image")
            .removeClass("d-none");
        
        formVerboseErrors();
        $("#errors-verbose").removeClass("d-none");
    }
    else{
        errorText
            .text("Нет ошибок")
            .addClass("text-success");
        results
            .find("#success-image")
            .removeClass("d-none");
    }
}

function formVerboseErrors() {
    let incorrectQuestions = $(".question.incorrect");
    let verboseUl = $("#errors-verbose-ul");
    
    incorrectQuestions.each(function (i) {
        let question = $(incorrectQuestions[i]);
        let questionValue = question.find(".question-value").text();
        let selectedAnswers = question
            .find(".answer")
            .filter(function(y){                
                return $(this).find(".answer-checkbox").is(':checked');                
            });
        let correctAnswers = question
            .find(".answer")
            .filter(function(y){
                return $(this).data("correct") === "True";
            });
        
        let questionDiv = `
<div class="">
 <span class="font-weight-bold">Вопрос: </span>
 <div>${questionValue}</div>
</div>`;
        
        let selectedDivs = "";
        selectedAnswers.each(function (x) {
            let answText = $(this).find(".answer-text").text();
            selectedDivs+= `<div>${answText}</div>`            
        });        
        let selectedDiv = `
<div class="">
 <span class="font-weight-bold">Выбран ответ: </span>
 ${selectedDivs}
</div>`;

        let correctDivs = "";
        correctAnswers.each(function (x) {
            let answText = $(this).find(".answer-text").text();
            correctDivs+= `<div>${answText}</div>`
        });
        let correctDiv = `
<div class="">
 <span class="font-weight-bold">Правильный ответ: </span>
 ${correctDivs}
</div>`;
        
        let li = $("<li class='list-group-item'></li>").append(questionDiv, selectedDiv, correctDiv);  
        verboseUl.append(li);
    });

}

function reset() {
    location.reload();
}

function gogopowerrangers() {
    $(this).closest("form").submit();
}
