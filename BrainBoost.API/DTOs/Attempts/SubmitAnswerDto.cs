namespace BrainBoost.API.DTOs.Attempts;

public class SubmitAnswerDto
{
    public int QuestionId { get; set; }

    public int SelectedOptionId { get; set; }
}