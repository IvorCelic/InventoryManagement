import { useEffect, useState } from "react";
import { Button, Container, Table } from "react-bootstrap";
import { useNavigate } from "react-router-dom";
import { FaEdit, FaTrash } from "react-icons/fa";
import LocationService from "../../services/LocationService";
import { RoutesNames } from "../../constants";
import SearchAndAdd from "../../components/SearchAndAdd";

export default function Locations() {
    const [locations, setLocations] = useState();
    const navigate = useNavigate();

    async function fetchLocations() {
        await LocationService.get()
            .then((res) => {
                setLocations(res.data);
            })
            .catch((e) => {
                alert(e);
            });
    }

    async function removeLocation(id) {
        const response = await LocationService.remove(id)
        if (response.ok) {
            alert(response.message.data.message);
            fetchLocations();
        }
    }

    useEffect(() => {
        fetchLocations();
    }, []);

    return (
        <Container>
            <Container>
                <SearchAndAdd RouteName={RoutesNames.LOCATIONS_CREATE} entity={"location"}/>
            </Container>
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
                                        onClick={() => { navigate(`/locations/${location.id}`) }}
                                    >
                                        <FaEdit
                                            size={25}
                                        />
                                    </Button>
                                    <Button
                                        className='link-danger actionButton'
                                        onClick={() => removeLocation(location.id)}
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
