using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using System;
using System.ComponentModel.DataAnnotations;

namespace OQPYBot.Controllers
{
    public class DialogAddReview
    {
        public static IDialog<AddReview> Create(IDialogContext context)
        {
            var form = Chain.From(() =>
            {
                return FormDialog.FromForm(AddReview.Builder);
            });
            var message = context.MakeMessage();
            return form;
        }
    }

    [Serializable]
    public class AddReview
    {
        [Prompt("Write your review")]
        public string Review { get; set; }

        [Prompt("Add your grade (0 - 10)")]
        [Range(0, 10)]
        public int Grade { get; set; }

        public static IForm<AddReview> Builder()
        {
            var colture = System.Threading.Thread.CurrentThread.CurrentUICulture;
            var build = new FormBuilder<AddReview>()
                    .Message("Tell me what you think")
                    .Field(nameof(Review))
                    .Field(nameof(Grade))
                    .Build();
            return build;
        }
    }
}