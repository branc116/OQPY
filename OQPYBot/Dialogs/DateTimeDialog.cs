using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using System;
using System.ComponentModel.DataAnnotations;

namespace OQPYBot.Dialogs
{
    public class DateTimePickerDialog
    {
        public static IDialog<DateTimePicker> Create(IDialogContext context)
        {
            var form = Chain.From(() =>
            {
                return FormDialog.FromForm(DateTimePicker.Builder);
            });
            return form;
        }
    }

    [Serializable]
    public class DateTimePicker
    {
        [Prompt("Enter Time and date: ")]
        [Template(TemplateUsage.DateTimeHelp)]
        public DateTime Time { get; set; }

        [Prompt("How long will you stay?")]
        [Template(TemplateUsage.DateTime)]
        [Range(1, 8)]
        public double Duration { get; set; }

        public static IForm<DateTimePicker> Builder()
        {
            var colture = System.Threading.Thread.CurrentThread.CurrentUICulture;
            var build = new FormBuilder<DateTimePicker>()
                    .Field(nameof(Time), validate: async (ctx, arg1) =>
                    {
                        return new ValidateResult() { IsValid = true, Value = arg1 };
                    })
                    .Field(nameof(Duration))
                    .Build();
            return build;
        }
    }
}