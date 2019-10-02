using System;
using NSW.EliteDangerous.API.Exceptions;

namespace NSW.EliteDangerous.Copilot.Models
{
    public class ErrorModel
    {
        public string TimeStamp { get; }
        public string Type { get; }
        public string Message { get; }
        public string ExceptionMessage { get; }

        public ErrorModel(JournalException exception)
        {
            ExceptionMessage = exception.Message;
            TimeStamp = DateTime.Now.ToString("yy-MM-dd hh:mm:ss");

            switch (exception)
            {
                case JournalRecordException jre:
                    Type = "Ошибка чтения строки журнала";
                    Message = jre.JournalRecord;
                    break;
                case JournalEventNotFoundException jenfe:
                    Type = $"Неизвестный евент {jenfe.EventName}";
                    Message = jenfe.JournalRecord;
                    break;
                case JournalFileException jfe:
                    Type = "Ошибка чтения файла";
                    Message = jfe.FileName;
                    break;
                case JournalFileNotFoundException jfnfe:
                    Type = "Файл не найден";
                    Message = jfnfe.FileName;
                    break;
                default:
                    Type = $"Ошибка работы API '{exception.Type}'";
                    Message = exception.Message;
                    break;
            }
        }
    }
}