using System;
using ServiceContract.DTO;
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
	}
}

