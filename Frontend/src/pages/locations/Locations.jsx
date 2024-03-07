import { useEffect, useState } from "react";
import { Button, Container, Table } from "react-bootstrap";
import { Link, useNavigate } from "react-router-dom";
import { FaEdit, FaTrash } from "react-icons/fa";
import LocationService from "../../services/LocationService";
import { GrFormAdd } from "react-icons/gr";
import { RoutesNames } from "../../constants";

export default function Locations() {
    const [locations, setLocations] = useState();

    async function fetchLocations() {
        await LocationService.getLocations()
            .then((res) => {
                setLocations(res.data);
            })
            .catch((e) => {
                alert(e);
            });
    }

    async function handleLocationDelete(id) {
        const response = await LocationService.deleteLocation(id)
        if (response.ok) {
            console.log(response.message);
            fetchLocations();
        }
    }

    useEffect(() => {
        fetchLocations();
    }, []);


    return (
        <Container>
            <Link
                to={RoutesNames.LOCATIONS_NEW}
                className="btn btn-primary myButton addButton"
            >
                <GrFormAdd
                    size={30}
                /> Add new location
            </Link>
            <Table striped bordered hover responsive>
                <thead>
                    <tr>
                        <th>Name</th>
                        <th>Description</th>
                        <th>Action</th>
                    </tr>
                </thead>
                <tbody>
                    {locations && locations.map((location, index) => (
                        <tr key={index}>
                            <td>{location.name}</td>
                            <td>{location.description}</td>
                            <td>
                                <Container
                                    className="d-flex justify-content-center"
                                >
                                    <Button
                                        variant="link"
                                        className="me-2 actionButton"
                                    >
                                        <FaEdit
                                            size={25}
                                        />
                                    </Button>
                                    <Button
                                        className='link-danger actionButton'
                                        onClick={() => handleLocationDelete(location.id)}
                                    >
                                        <FaTrash
                                            size={25}
                                        />
                                    </Button>
                                </Container>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </Table>
        </Container>
    );
}
