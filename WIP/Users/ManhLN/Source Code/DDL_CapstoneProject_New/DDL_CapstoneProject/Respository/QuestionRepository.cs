using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DDL_CapstoneProject.Helpers;
using DDL_CapstoneProject.Models;
using DDL_CapstoneProject.Models.DTOs;
using DDL_CapstoneProject.Ultilities;

namespace DDL_CapstoneProject.Respository
{
    public class QuestionRepository : SingletonBase<QuestionRepository>
    {

        #region "Constructors"
        private QuestionRepository()
        {
        }
        #endregion

        #region "Methods"

        /// <summary>
        /// Get question list of a project
        /// </summary>
        /// <returns>questionList</returns>
        public List<QuestionDTO> GetQuestion(int ProjectID)
        {
            using (var db = new DDLDataContext())
            {
                // Get rewardPkg list
                var questionList = (from Question in db.Questions
                                    where Question.ProjectID == ProjectID
                                    orderby Question.CreatedDate ascending
                                    select new QuestionDTO
                                    {
                                        Answer = Question.Answer,
                                        CreatedDate = Question.CreatedDate,
                                        QuestionContent = Question.QuestionContent,
                                        QuestionID = Question.QuestionID
                                    }).ToList();

                questionList.ForEach(x => x.CreatedDate = CommonUtils.ConvertDateTimeFromUtc(x.CreatedDate));

                return questionList;
            }
        }

        /// <summary>
        /// Create a new QA
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="question"></param>
        /// <returns>newQuestionDTO</returns>
        public QuestionDTO CreateQuestion(int ProjectID, QuestionDTO question)
        {
            using (var db = new DDLDataContext())
            {
                var newQuestion = db.Questions.Create();
                newQuestion.ProjectID = ProjectID;
                newQuestion.CreatedDate = DateTime.UtcNow;
                newQuestion.QuestionContent = question.QuestionContent;
                newQuestion.Answer = question.Answer;

                db.Questions.Add(newQuestion);
                db.SaveChanges();

                var newQuestionDTO = new QuestionDTO
                {
                    Answer = newQuestion.Answer,
                    CreatedDate = newQuestion.CreatedDate,
                    QuestionContent = newQuestion.QuestionContent,
                    QuestionID = newQuestion.QuestionID
                };

                newQuestionDTO.CreatedDate = CommonUtils.ConvertDateTimeFromUtc(newQuestionDTO.CreatedDate);

                return newQuestionDTO;
            }
        }

        /// <summary>
        /// Edit QAs
        /// </summary>
        /// <returns>boolean</returns>
        public bool EditQuestion(List<QuestionDTO> question)
        {
            using (var db = new DDLDataContext())
            {
                foreach (var qa in question)
                {
                    var updateQuestion = db.Questions.SingleOrDefault(x => x.QuestionID == qa.QuestionID);

                    if (updateQuestion == null)
                    {
                        throw new KeyNotFoundException();
                    }

                    updateQuestion.Answer = qa.Answer;
                    updateQuestion.CreatedDate = DateTime.UtcNow;
                    updateQuestion.QuestionContent = qa.QuestionContent;

                    db.SaveChanges();
                }

                return true;
            }
        }

        /// <summary>
        /// Delete a QA
        /// </summary>
        /// <param name="questionID"></param>
        /// <returns>boolean</returns>
        public bool DeleteQuestion(int questionID)
        {
            using (var db = new DDLDataContext())
            {
                var deleteQuestion = db.Questions.SingleOrDefault(x => x.QuestionID == questionID);

                if (deleteQuestion == null)
                {
                    throw new KeyNotFoundException();
                }

                db.Questions.Remove(deleteQuestion);
                db.SaveChanges();

                return true;
            }
        }

        #endregion
    }
}