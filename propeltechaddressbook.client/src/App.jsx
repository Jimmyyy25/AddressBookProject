import { useEffect, useState, useRef } from 'react';
import ABLineModal from "./components/ABLineModal";
import './App.css';

function App() {

    const apiBaseUrl = import.meta.env.VITE_API_BASE_URL;

    const [addressBook, setAddressBook] = useState(null);
    const [message, setMessage] = useState(null);

    const [showModal, setShowModal] = useState(false);
    const [editingEntry, setEditingEntry] = useState(null);

    useEffect(() => {
        populateAddressBook();
    }, []);

    // Event handlers
    async function onEditClick(email) {

        console.log("onEditClick()", email);

        const line = await getAddressBookLine(email);

        setEditingEntry(line);
        setShowModal(true);

        return;

    }

    function onCreateClick() {

        console.log("onCreateClick()");

        setEditingEntry(null); // means new record
        setShowModal(true);

        return;
    }

    function onDeleteClick(email) {

        console.log("onDeleteClick()", email);

        // TODO: prompt are you sure?

        deleteAddressBookLine(email)

        return;
    }

    // Event handlers end

    console.log("addressBook", addressBook);

    const tableRef = useRef(null);

    // could use jquery datatables for searching and sorting
    //tableRef = new DataTable('#addressBookTable');

    return (

        <>

            <h1 id="tableLabel" className="mb-4">Address Book</h1>

            {
                (!message && !addressBook)
                    ?
                    <p><em>Loading address book. Please wait...</em></p>
                    :
                    addressBook
                        ?
                        <div className="mx-auto mb-3 d-flex flex-column">
                            <table ref={tableRef} id="addressBookTable" className="table table-striped" aria-labelledby="tableLabel">
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
                                                <td>
                                                    <button type="button" className="btn btn-sm btn-secondary me-2" onClick={() => onEditClick(line.email)}>Edit</button>
                                                    <button type="button" className="btn btn-sm btn-danger" onClick={() => onDeleteClick(line.email)}>Delete</button>
                                                </td>
                                            </tr>
                                        )
                                    }
                                </tbody>
                            </table>
                            <button type="button" className="btn btn-sm btn-success ms-auto" onClick={onCreateClick}>Add</button>
                        </div>
                        :
                        <p><em>{message}</em></p>
            }

            {
                showModal
                &&
                    <ABLineModal title="Edit Contact" model={editingEntry} onClose={() => setShowModal(false)} onSave={handleSave}>
                        
                   </ABLineModal>
            }

        </>
    );

    async function populateAddressBook() {

        const response = await fetch(`${apiBaseUrl}/AddressBook/GetAll`);

        if (response.ok) {

            const data = await response.json();

            console.log("populateAddressBook data ", data);


            if (data.isSuccess)
                setAddressBook(data.payload);
            else
                setMessage(data.message)
        }
    }

    async function getAddressBookLine(email) {

        const encodedEmail = encodeURIComponent(email);

        const response = await fetch(`${apiBaseUrl}/AddressBook/GetByEmail?email=${encodedEmail}`);

        if (response.ok) {

            const data = await response.json();

            console.log("populateAddressBook data ", data);


            if (data.isSuccess)
                return data.payload;
            else
                setMessage(data.message)
        }
    }

    async function handleSave(object) {

        const jsonObject = JSON.stringify(object);

        const response = await fetch(`${apiBaseUrl}/AddressBook/Create`, {
            method: "DELETE",
            body: jsonObject,
            headers: {
                "Content-Type": "application/json"
            }
        });

        if (response.ok) {

            const data = await response.json();


            console.log("deleteAddressBookLine data ", data);

            if (data.isSuccess) {
                // could/should show toast
                // e.g. setToast(data.message);

                populateAddressBook();
            }
            else {
                setMessage(data.message)
            }
        }
    }

    async function deleteAddressBookLine(email) {

        const encodedEmail = encodeURIComponent(email);

        const response = await fetch(`${apiBaseUrl}/AddressBook/Delete/${encodedEmail}`, {
            method: "DELETE"
        });

        if (response.ok) {

            const data = await response.json();
            

            console.log("deleteAddressBookLine data ", data);

            if (data.isSuccess) {
                // could/should show toast
                // e.g. setToast(data.message);

                populateAddressBook();
            }
            else {
                setMessage(data.message)
            }
        }
    }
}

export default App;