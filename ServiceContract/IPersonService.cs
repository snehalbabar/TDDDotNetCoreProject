using System;
using ServiceContract.DTO;
using ServiceContract.Enums;

namespace ServiceContract
{
	/// <summary>
	/// Represnts business logic for manipulating person entity
	/// </summary>
	public interface IPersonService
	{ 
		/// <summary>
		/// adds a new person into thhe list of persons 
		/// </summary>
		/// <param name="personAddRequest">person to add</param>
		/// <returns>Returns the same person details along with newly generated PersonId</returns>
		PersonResponse AddPerson(PersonAddRequest? personAddRequest);

		/// <summary>
		/// get list of all person in the database. 
		/// </summary>
		/// <returns></returns>
		List<PersonResponse> GetAllPerson();

		/// <summary>
		/// return the person objcet based on the given person id
		/// </summary>
		/// <param name="personId"></param>
		/// <returns>matching person object</returns>
		PersonResponse? GetPersonByPersonId(Guid? personId);

		/// <summary>
		/// return all person objects that matches with the given search field and search string
		/// </summary>
		/// <param name="searchBy">search field to seach</param>
		/// <param name="searchString">ssearch string to search</param>
		/// <returns>returns all matching persons based on the given search field and serach string</returns>
		List<PersonResponse> GetFilteredPerson(String searchBy, string? searchString);

		/// <summary>
		/// return sorted list of person
		/// </summary>
		/// <param name="people"> Repersent list of persons to sorted</param>
		/// <param name="sortBy">Name of the property(Key),based on which the persons should be sorted</param>
		/// <param name="sortOrder">ASC or DESC</param>
		/// <returns>Returns soretd person list</returns>
		List<PersonResponse> GetSortedPersons(List<PersonResponse> people, string sortBy, SortOrderOptions sortOrder);

		/// <summary>
		/// updates the specified person details based on the given person Id
		/// </summary>
		/// <param name="request">person details to update inclueding person id</param>
		/// <returns>Returns the person response object after updation.</returns>
		PersonResponse UpdatePerson(PersonUpdateRequest? request);

		/// <summary>
		/// Deletes a person based on the given person id
		/// </summary>
		/// <param name="peronId">person id to delete</param>
		/// <returns>Returns true, if the deletion is successful; otherwise return false</returns>
		bool DeletePerson(Guid? peronId);

	}
}

