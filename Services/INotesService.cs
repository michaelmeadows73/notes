using NotesApi.Dtos;

namespace NotesApi.Services;

public interface INotesService
{
    IEnumerable<NoteDto> GetNotes();

    NoteDto GetNote(Guid noteIdentifier);

    NoteDto CreateNote(NoteCreateUpdateDto noteCreateUpdateDto);

    NoteDto UpdateNote(Guid noteIdentifier, NoteCreateUpdateDto noteCreateUpdateDto);

    void DeleteNote(Guid noteIdentifier);
}
