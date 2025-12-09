import { useEffect, useState, useRef } from 'react';
import './App.css';

function App() {

    const apiBaseUrl = import.meta.env.VITE_API_BASE_URL;

    const [addressBook, setAddressBook] = useState(null);
    const [message, setMessage] = useState(null);

    useEffect(() => {
        populateAddressBookData();
    }, []);

    // Event handlers
    function onEditClick(email) {

        console.log("onEditClick()", email);

        return;

    }

    function onSaveClick() {

        console.log("onSaveClick()");
        return;
    }

    function onDeleteClick(email) {

        console.log("onDeleteClick()", email);
        return;
    }

    // Event handlers end

    console.log("addressBook", addressBook);

    return (

        <>

            <h1 id="tableLabel">Address Book</h1>

            {
                (!message && !addressBook)
                ?
                   <p><em>Loading address book. Please wait...</em></p>
                :
                    addressBook 
                    ?
                        <div className="mx-auto">
                            <table id="addressBookTable" className="table table-striped" aria-labelledby="tableLabel">
                                <thead>
                                    <tr>
                                        <th>First Name</th>
                                        <th>Last Name</th>
                                        <th>Phone</th>
                                        <th>Email</th>
                                        <th>Actions</th>
                                    </tr>
                                </thead>

                                <tbody>
                                {
                                        addressBook.map((line, index) =>
                                        <tr key={index}>
                                            <td>{line.first_name}</td>
                                            <td>{line.last_name}</td>
                                            <td>{line.phone}</td>
                                                <td>{line.email}</td>
                                                <td><button className="btn-sm" onClick={onEditClick(line.email)}>Edit</button></td>
                                                <td><button className="btn-sm" onClick={onDeleteClick(line.email)}>Delete</button></td>
                                        </tr>
                                    )
                                }
                                </tbody>
                            </table>
                        </div>
                    :
                        <p><em>{message}</em></p>
            }

        </>
    );
    
    async function populateAddressBookData() {

        const response = await fetch(`${apiBaseUrl}/AddressBook/GetAll`);

        if (response.ok) {

            const data = await response.json();

            console.log("data ", data);


            if (data.isSuccess)
                setAddressBook(data.payload);
            else
                setMessage(data.message)
        }
    }
}

export default App;