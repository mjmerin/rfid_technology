using System;

/// <summary>
/// Summary description for Class1
/// </summary>

[Serializable]
public class PatientData
{
    //Patient Data properties
    public string name;
    public string gender;
    public Int16 age;
    public string address;
    public string city;
    public string state;
    public string zipcode;

    //Default Constructor
	public PatientData()
	{
		// Default Values
        name = "N/A";
        gender = "M";
        age = 0;
        address = "N/A";
        city = "N/A";
        state = "N/A";
        zipcode = "N/A";
	}

    public PatientData(string initName, string initGender, string initAge, string initAddress, string initCity, string initState, string initZipcode)
    //public PatientData(string initName, string initGender, string initAge)    
    {
        name = initName;
        gender = initGender;
        age = Int16.Parse(initAge);
        address = initAddress;
        city = initCity;
        state = initState;
        zipcode = initZipcode;
    }
}
