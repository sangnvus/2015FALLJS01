﻿using System;
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
        private DDLDataContext db;

        #region "Constructors"
        private QuestionRepository()
        {
            db = new DDLDataContext();
        }
        #endregion

        #region "Methods"

        /// <summary>
        /// Get question list of a project
        /// </summary>
        /// <returns>questionList</returns>
        public List<QuestionDTO> GetQuestion(int ProjectID)
        {
            // Get rewardPkg list
            var questionList = from Question in db.Questions
                               where Question.ProjectID == ProjectID
                               orderby Question.CreatedDate ascending
                               select new QuestionDTO
                               {
                                   Answer = Question.Answer,
                                   CreatedDate = Question.CreatedDate,
                                   QuestionContent = Question.QuestionContent,
                                   QuestionID = Question.QuestionID
                               };

            return questionList.ToList();
        }

        /// <summary>
        /// Create a new QA
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="question"></param>
        /// <returns>newQuestionDTO</returns>
        public QuestionDTO CreateQuestion(int ProjectID, QuestionDTO question)
        {
            var newQuestion = new Question
            {
                ProjectID = ProjectID,
                CreatedDate = DateTime.Today,
                QuestionContent = question.QuestionContent,
                Answer = question.Answer,
            };
            db.Questions.Add(newQuestion);
            db.SaveChanges();

            var newQuestionDTO = new QuestionDTO
            {
                Answer = newQuestion.Answer,
                CreatedDate = newQuestion.CreatedDate,
                QuestionContent = newQuestion.QuestionContent,
                QuestionID = newQuestion.QuestionID
            };

            return newQuestionDTO;
        }

        /// <summary>
        /// Edit QAs
        /// </summary>
        /// <returns>boolean</returns>
        public bool EditQuestion(List<QuestionDTO> question)
        {
            foreach (var qa in question)
            {
                var updateQuestion = db.Questions.SingleOrDefault(x => x.QuestionID == qa.QuestionID);

                if (updateQuestion == null)
                {
                    throw new KeyNotFoundException();
                }

                updateQuestion.Answer = qa.Answer;
                updateQuestion.CreatedDate = DateTime.Today;
                updateQuestion.QuestionContent = qa.QuestionContent;

                db.SaveChanges();
            }

            return true;
        }

        /// <summary>
        /// Delete a QA
        /// </summary>
        /// <param name="questionID"></param>
        /// <returns>boolean</returns>
        public bool DeleteQuestion(int questionID)
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

        #endregion
    }
}