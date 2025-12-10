import { useEffect, useRef } from 'react';

function ABLineModal({ title, model, children }) {

    const modalRef = useRef(null);
    const bsModalRef = useRef(null);

    useEffect(() => {
        // Ensure bootstrap is available globally
        if (!window.bootstrap) {
            console.error("Bootstrap JS not loaded!");
            return;
        }
        else {
            console.error("Bootstrap JS OK!");

        }

        // Load Bootstrap modal JS
        console.error("Bootstrap JS OK! 1");

        bsModalRef.current = new window.bootstrap.Modal(modalRef.current); // horrible js backdrop error here!
        bsModalRef.current?.show();

        console.error("Bootstrap JS OK! 2");


        return () => {
            bsModalRef.current?.hide();
        };
    }, []);

    const open = () => bsModalRef.current?.show();
    const close = () => bsModalRef.current?.hide();

    return
    (
        <div className="modal fade" id="ABLineModal" tabIndex="-1" aria-hidden="true" ref={modalRef}>
            <div className="modal-dialog modal-dialog-centered">
                <div className="modal-content">

                    <div className="modal-header">
                        <h5 className="modal-title">{title}</h5>
                        <button type="button" className="btn-close" onClick={close} aria-label="Close"></button>
                    </div>

                    <div className="modal-body">
                        {/*{children}*/}
                        <form onSubmit={handleSubmit}>

                            <div className="mb-3">
                                <label className="form-label">First Name</label>
                                <input
                                    type="text"
                                    name="firstName"
                                    className="form-control"
                                    value={model.firstName}
                                    onChange={handleChange}
                                />
                            </div>

                            <div className="mb-3">
                                <label className="form-label">Last Name</label>
                                <input
                                    type="text"
                                    name="lastName"
                                    className="form-control"
                                    value={model.lastName}
                                    onChange={handleChange}
                                />
                            </div>

                            <div className="mb-3">
                                <label className="form-label">Phone</label>
                                <input
                                    type="text"
                                    name="phone"
                                    className="form-control"
                                    value={model.phone}
                                    onChange={handleChange}
                                />
                            </div>

                            <div className="mb-3">
                                <label className="form-label">Email (read only)</label>
                                <input
                                    type="email"
                                    name="email"
                                    className="form-control"
                                    value={model.email}
                                    readOnly
                                />
                            </div>

                            <div className="d-flex justify-content-end">
                                <button
                                    type="button"
                                    className="btn btn-secondary me-2"
                                    onClick={onClose}
                                >
                                    Cancel
                                </button>

                                <button type="submit" className="btn btn-primary">
                                    Save Changes
                                </button>
                            </div>

                        </form>
                    </div>

                    <div className="modal-footer">
                        <button type="button" className="btn btn-secondary" onClick={close}>Close</button>
                    </div>

                </div>
            </div>
        </div>
    );

}

export default ABLineModal;