using Azure;
using Azure.Data.Tables;
using NotesApi.Dtos;
using NotesApi.Exceptions;

namespace NotesApi.Services;

public class TableNotesService : INotesService
{
    private readonly TableClient _tableClient;

    public TableNotesService(IConfiguration configuration)
    {
        string storageUri = configuration["StorageUri"]!;
        string storageAccountName = configuration["StorageAccountName"]!;
        string storageAccountKey = configuration["StorageAccountKey"]!;

        _tableClient = new TableClient(new Uri(storageUri), "Notes", new TableSharedKeyCredential(storageAccountName, storageAccountKey));
    }

    public IEnumerable<NoteDto> GetNotes()
    {
        Pageable<TableEntity> results = _tableClient.Query<TableEntity>();

        return results.Select(r => new NoteDto 
        {
            NoteIdentifier = (Guid) r.GetGuid("NoteIdentifier")!,
            NoteText = r.GetString("NoteText")
        });
    }

    public NoteDto GetNote(Guid noteIdentifier)
    {
        Pageable<TableEntity> results = _tableClient.Query<TableEntity>(filter: $"PartitionKey eq '{noteIdentifier}'");

        NoteDto? noteDto = results.
            Select(r => new NoteDto 
            {
                NoteIdentifier = (Guid) r.GetGuid("NoteIdentifier")!,
                NoteText = r.GetString("NoteText")
            }).
            FirstOrDefault();

        if (noteDto == null)
        {
            throw new NoteNotFoundException();
        }

        return noteDto;
    }

    public NoteDto CreateNote(NoteCreateUpdateDto noteCreateUpdateDto)
    {
        Guid noteIdentifier = Guid.NewGuid();

        var tableEntity = new TableEntity(noteIdentifier.ToString(), noteIdentifier.ToString())
        {
            { "NoteIdentifier", noteIdentifier },
            { "NoteText", noteCreateUpdateDto.NoteText }
        };

        // Add the newly created entity.
        _tableClient.AddEntity(tableEntity);

        return new NoteDto { NoteIdentifier = noteIdentifier, NoteText = noteCreateUpdateDto.NoteText };
    }


    public NoteDto UpdateNote(Guid noteIdentifier, NoteCreateUpdateDto noteCreateUpdateDto)
    {
        Pageable<TableEntity> results = _tableClient.Query<TableEntity>(filter: $"PartitionKey eq '{noteIdentifier}'");

        TableEntity? noteEntity = results.FirstOrDefault();

        if (noteEntity == null)
        {
            throw new NoteNotFoundException();
        }

        noteEntity["NoteText"] = noteCreateUpdateDto.NoteText;

        _tableClient.UpdateEntity(noteEntity, ETag.All);

        return new NoteDto { NoteIdentifier = noteIdentifier, NoteText = noteCreateUpdateDto.NoteText };
    }

    public void DeleteNote(Guid noteIdentifier)
    {
        _tableClient.DeleteEntity(noteIdentifier.ToString(), noteIdentifier.ToString());
    }
}
