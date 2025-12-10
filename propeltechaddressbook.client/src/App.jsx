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
    const formRef = useRef();

    const emptyForm = {
        first_name: "",
        last_name: "",
        phone: "",
        email: ""
    };

    const [form, setForm] = useState({
        first_name: "",
        last_name: "",
        phone: "",
        email: ""
    });

    useEffect(() => {
        if (editingEntry)
            setForm(editingEntry);
        else
            setForm(emptyForm);
    }, [editingEntry]);

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

    const handleSubmit = async (e) => {
        e.preventDefault();

        const formData = new FormData(formRef.current);
        const json = Object.fromEntries(formData.entries());

        if (editingEntry)
            updateAddressBookLine(json);
        else
            createAddressBookLine(json);
    };

    // Event handlers end

    console.log("addressBook", addressBook);

    const tableRef = useRef(null);
    const modalRef = useRef(null);
    const closeModalRef = useRef(null);

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
                                                    <button type="button" data-bs-toggle="modal" data-bs-target="#ABLineModal" className="btn btn-sm btn-secondary me-2" onClick={() => onEditClick(line.email)}>Edit</button>
                                                    <button type="button" className="btn btn-sm btn-danger" onClick={() => onDeleteClick(line.email)}>Delete</button>
                                                </td>
                                            </tr>
                                        )
                                    }
                                </tbody>
                            </table>
                            <button type="button" data-bs-toggle="modal" data-bs-target="#ABLineModal" className="btn btn-sm btn-success ms-auto" onClick={onCreateClick}>Add</button>
                        </div>
                    :
                        <p><em>{message}</em></p>
            }

            {
                message
                &&
                <p className="text-secondary">{message}</p>
            }

            <div className="modal fade" id="ABLineModal" tabIndex="-1" aria-hidden="true" ref={modalRef}>
                <div className="modal-dialog modal-dialog-centered">
                    <div className="modal-content">

                        <div className="modal-header">
                            <h5 className="modal-title">{editingEntry ? "Edit Line" : "Add Line"}</h5>
                            <button type="button" className="btn-close" data-bs-dismiss="modal" aria-label="Close" ref={closeModalRef}></button>
                        </div>

                        <div className="modal-body text-start">

                            <form ref={formRef} onSubmit={handleSubmit}>

                                <div className="mb-3">
                                    <label className="form-label">First Name</label>
                                    <input type="text" name="first_name" className="form-control" value={form.first_name ?? ""} onChange={(e) => setForm({ ...form, first_name: e.target.value })} />
                                </div>

                                <div className="mb-3">
                                    <label className="form-label">Last Name</label>
                                    <input type="text" name="last_name" className="form-control" value={form.last_name ?? ""} onChange={(e) => setForm({ ...form, last_name: e.target.value })} />
                                </div>

                                <div className="mb-3">
                                    <label className="form-label">Phone</label>
                                    <input type="text" name="phone" className="form-control" value={form.phone ?? ""} onChange={(e) => setForm({ ...form, phone: e.target.value })} />
                                </div>

                                <div className="mb-3">
                                    <label className="form-label">Email</label>
                                    <input type="email" name="email" className="form-control" value={form.email ?? ""} onChange={(e) => setForm({ ...form, email: e.target.value })} />
                                </div>

                                <div className="d-flex justify-content-end">

                                    <button type="button" className="btn btn-secondary me-2" data-bs-dismiss="modal">
                                        Cancel
                                    </button>

                                    <button type="submit" className="btn btn-primary">
                                        Save Changes
                                    </button>

                                </div>

                                <p><em>{message}</em></p>

                            </form>

                        </div>
                    </div>
                </div>
            </div>

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

            console.log("getAddressBookLine data ", data);


            if (data.isSuccess)
                return data.payload;
            else
                setMessage(data.message)
        }
    }

    //async function handleSave(object) {

    //    const jsonObject = JSON.stringify(object);

    //    const response = await fetch(`${apiBaseUrl}/AddressBook/Create`, {
    //        method: "POST",
    //        body: jsonObject,
    //        headers: {
    //            "Content-Type": "application/json"
    //        }
    //    });

    //    if (response.ok) {

    //        const data = await response.json();


    //        console.log("handleSave data ", data);

    //        if (data.isSuccess) {
    //            // could/should show toast
    //            // e.g. setToast(data.message);

    //            populateAddressBook();
    //        }
    //        else {
    //            setMessage(data.message)
    //        }
    //    }
    //}

    async function createAddressBookLine(json) {

        const response = await fetch(`${apiBaseUrl}/AddressBook/Create`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(json)
        });

        if (response.ok) {

            const data = await response.json();

            console.log("createAddressBookLine data ", data);

            if (data.isSuccess) {
                // could/should show toast
                // e.g. setToast(data.message);

                populateAddressBook();
                closeModalRef.current.click();
            }
            else {
                setMessage(data.message)
            }
        }
    }

    async function updateAddressBookLine(json) {

        const response = await fetch(`${apiBaseUrl}/AddressBook/Update`, {
            method: "PUT",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(json)
        });

        if (response.ok) {

            const data = await response.json();

            console.log("updateAddressBookLine data ", data);

            if (data.isSuccess) {
                // could/should show toast
                // e.g. setToast(data.message);

                populateAddressBook();
                closeModalRef.current.click();
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