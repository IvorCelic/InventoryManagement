import { useEffect, useRef, useState } from "react";
import { Button, Col, Container, Form, Image, Row } from "react-bootstrap";
import { useNavigate, useParams } from "react-router-dom";
import { App, RoutesNames } from "../../constants";
import EmployeeService from "../../services/EmployeeService";
import useError from "../../hooks/useError";
import ActionButtons from "../../components/ActionButtons";
import useLoading from "../../hooks/useLoading";
import unknown from "../../assets/unknown.png";
import { Cropper } from "react-cropper";
import "cropperjs/dist/cropper.css";

export default function EmployeesEdit() {
    const entityName = "employee";
    const navigate = useNavigate();
    const routeParams = useParams();
    const { showError } = useError();
    const cropperRef = useRef(null);

    const [employee, setEmployee] = useState({});
    const { showLoading, hideLoading } = useLoading();

    const [currentImage, setCurrentImage] = useState("");
    const [imageForCrop, setImageForCrop] = useState("");
    const [imageForServer, setImageForServer] = useState("");

    async function fetchEmployee() {
        showLoading();
        const response = await EmployeeService.getById("Employee", routeParams.id);
        if (!response.ok) {
            showError(response.data);
            navigate(RoutesNames.EMPLOYEES_LIST);
            return;
        }
        setEmployee(response.data);

        if (response.data.image != null) {
            setCurrentImage(App.URL + response.data.image + `?${Date.now()}`);
        } else {
            setCurrentImage(unknown);
        }
        hideLoading();
    }

    useEffect(() => {
        fetchEmployee();
    }, []);

    async function editEmployee(entityName) {
        showLoading();
        const response = await EmployeeService.edit("Employee", routeParams.id, entityName);
        if (response.ok) {
            hideLoading();
            // console.log(response.data);
            navigate(RoutesNames.EMPLOYEES_LIST);
            return;
        }
        showError(response.data);
        hideLoading();
    }

    function handleSubmit(event) {
        event.preventDefault();
        const data = new FormData(event.target);

        editEmployee({
            firstName: data.get("firstName"),
            lastName: data.get("lastName"),
            email: data.get("email"),
            password: data.get("password"),
            image: ""
        });
    }

    function onCrop() {
        setImageForServer(cropperRef.current.cropper.getCroppedCanvas().toDataURL());
    }

    function onChangeImage(event) {
        event.preventDefault();

        let files;
        if (event.dataTransfer) {
            files = event.dataTransfer.files;
        } else if (event.target) {
            files = event.target.files;
        }
        const reader = new FileReader();
        reader.onload = () => {
            setImageForCrop(reader.result);
        };
        try {
            reader.readAsDataURL(files[0]);
        } catch (error) {
            console.log(error);
        }
    }

    async function saveImage() {
        showLoading();
        const base64 = imageForServer;

        const response = await EmployeeService.SetImage(routeParams.id, {
            Base64: base64.replace("data:image/png;base64,", ""),
        });
        if (!response.ok) {
            hideLoading();
            showError(response.data);
        }
        console.log("Image saved successfully: ", response.data);

        setCurrentImage(imageForServer);
        hideLoading();
    }

    return (
        <Container className="square border mt-5">
            <Row>
                <Col key="1" md={6}>
                    <Row>
                        <h2 className="mt-5 ms-5">Edit {entityName}</h2>
                    </Row>
                    <Form className="m-5" onSubmit={handleSubmit}>
                        <Row>
                            <Col>
                                <Form.Group controlId="firstName">
                                    <Form.Label>First name</Form.Label>
                                    <Form.Control
                                        type="text"
                                        defaultValue={employee.firstName}
                                        name="firstName"
                                    />
                                </Form.Group>
                            </Col>
                            <Col>
                                <Form.Group controlId="lastName">
                                    <Form.Label>Last name</Form.Label>
                                    <Form.Control
                                        type="text"
                                        defaultValue={employee.lastName}
                                        name="lastName"
                                    />
                                </Form.Group>
                            </Col>
                        </Row>
                        <Form.Group controlId="email">
                            <Form.Label className="pt-4">Email</Form.Label>
                            <Form.Control type="text" defaultValue={employee.email} name="email" />
                        </Form.Group>
                        <Form.Group controlId="password">
                            <Form.Label className="pt-4">Password</Form.Label>
                            <Form.Control
                                type="password"
                                defaultValue={employee.password}
                                name="password"
                            />
                        </Form.Group>
                        <ActionButtons cancel={RoutesNames.EMPLOYEES_LIST} action="Save changes" />
                    </Form>
                </Col>
                <Col md={6}>
                    <Row className="mt-5">
                        <Col key="1" sm={12} lg={6} md={12}>
                            <p className="form-label">Trenutna slika</p>
                            <Image
                                // for local development
                                // src={'https://ivorcelic-001-site1.htempurl.com/' + currentImage}
                                src={currentImage}
                                className="image"
                            />
                        </Col>
                        <Col key="2" sm={12} lg={6} md={12}>
                            {imageForServer && (
                                <>
                                    <p className="form-label">New picture slika</p>
                                    <Image src={imageForServer || imageForCrop} className="image" />
                                </>
                            )}
                        </Col>
                    </Row>
                    <Row className="mt-4">
                        <Col key="2">
                            <input className="mb-3" type="file" onChange={onChangeImage} />
                            <Button disabled={!imageForServer} onClick={saveImage}>
                                Save Image
                            </Button>
                        </Col>
                    </Row>
                    <Row className="mb-5 me-5 mt-5">
                        <Col className="d-flex flex-column align-items-center">
                            <div className="cropper-container">
                                <Cropper
                                    src={imageForCrop}
                                    style={{ width: "100%", height: 400 }}
                                    initialAspectRatio={1}
                                    guides={true}
                                    viewMode={1}
                                    minCropBoxWidth={60}
                                    minCropBoxHeight={60}
                                    cropBoxResizable={false}
                                    background={false}
                                    responsive={true}
                                    checkOrientation={false}
                                    cropstart={onCrop}
                                    cropend={onCrop}
                                    ref={cropperRef}
                                />
                            </div>
                        </Col>
                    </Row>
                </Col>
            </Row>
        </Container>
    );
}
