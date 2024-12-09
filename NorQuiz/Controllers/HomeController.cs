using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.EF;
using WebApplication1.EF.Models;
using WebApplication1.Infrastructure;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private QuizContext Context { get; }

        public HomeController(QuizContext context)
        {
            Context = context;
        }

        [Route("")]
        public async Task<IActionResult> Index()
        {
            var quizes = await Context.Quizzes.Include(x => x.Questions).ToListAsync();
            return View(quizes);
        }

        [HttpPost("/[action]")]
        public async Task<IActionResult> CreateQuiz(Quiz quiz)
        {
            if (quiz == null)
            {
                RedirectToAction("Index");
            }

            Context.Add(quiz);
            await Context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet("/[action]/{id:int}")]
        public async Task<IActionResult> EditQuiz(int? id)
        {
            if (!id.HasValue)
            {
                RedirectToAction("Index");
            }

            var test = await Context.Quizzes
                .Where(x => x.Id == id)
                .Include(x => x.Questions)
                .ThenInclude(x => x.Answers)
                .FirstOrDefaultAsync();

            if (test == null)
            {
                TempData["Message"] = $"Тест с id${id} не найден.";
                return RedirectToAction("Index");
            }

            return View(test);
        }

        [HttpPost("/[action]")]
        public async Task<IActionResult> AddQuizQuestion(Question question)
        {
            if (question == null || question.QuizId == 0)
            {
                RedirectToAction("Index");
            }

            Context.Add(question);
            await Context.SaveChangesAsync();

            return RedirectToAction("EditQuiz", new {id = question.QuizId});
        }

        [HttpPost("/[action]")]
        public async Task<IActionResult> RemoveQuizQuestion(int? id)
        {
            if (id.HasValue)
            {
                RedirectToAction("Index");
            }

            var question = await Context.Questions.FindAsync(id.Value);
            if (question != null)
            {
                Context.Remove(question);
                await Context.SaveChangesAsync();
            }

            return RedirectToAction("EditQuiz", new {id = question?.QuizId});
        }

        [HttpPost("/[action]")]
        public async Task<IActionResult> AddQuestionAnswers(List<Answer> answers, int? quizId)
        {
            if (answers == null || !answers.Any() || answers.Any(x => x.QuestionId == 0) || !quizId.HasValue)
            {
                return RedirectToAction("Index");
            }

            Context.AddRange(answers);
            await Context.SaveChangesAsync();

            return RedirectToAction("EditQuiz", new {id = quizId.GetValueOrDefault()});
        }

        [HttpPost("/[action]")]
        public async Task<IActionResult> RemoveQuestionAnswer(int? id)
        {
            if (id.HasValue)
            {
                RedirectToAction("Index");
            }

            var answer = await Context.Answers
                .Where(x => x.Id == id.Value)
                .Include(x => x.QuestionNav)
                .FirstOrDefaultAsync();

            if (answer != null)
            {
                Context.Remove(answer);
                await Context.SaveChangesAsync();
            }

            return RedirectToAction("EditQuiz", new {id = answer.QuestionNav.QuizId});
        }

        [ValidateAntiForgeryToken]
        [HttpPost("/[action]")]
        public async Task<IActionResult> DeleteTest(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction("Index");
            }

            var test = await Context.Quizzes.Where(x => x.Id == id).FirstOrDefaultAsync();
            Context.Quizzes.Remove(test);
            await Context.SaveChangesAsync();

            TempData["Message"] = $"Тест {test.Name} успешно удален";
            return RedirectToAction("Index");
        }

        [HttpGet("/[action]")]
        public async Task<IActionResult> RunTest(int? id, int? questionCount)
        {
            if (!id.HasValue)
            {
                return RedirectToAction("Index");
            }

            var quiz = await Context.Quizzes
                .Where(x => x.Id == id.Value)
                .Include(x => x.Questions)
                .ThenInclude(x => x.Answers)
                .FirstOrDefaultAsync();

            if (quiz == null)
            {
                return RedirectToAction("Index");
            }

            var shuffledQeQuestions = quiz.Questions.ToList();
            shuffledQeQuestions.Shuffle();
            quiz.Questions = shuffledQeQuestions.ToArray();

            foreach (var quizQuestion in quiz.Questions)
            {
                var shuffledAnswers = quizQuestion.Answers.ToList();
                shuffledAnswers.Shuffle();
                quizQuestion.Answers = shuffledAnswers.ToArray();
            }

            var model = new RunTestModel()
            {
                QuestionCount = questionCount ?? 1,
                Quiz = quiz
            };
            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}