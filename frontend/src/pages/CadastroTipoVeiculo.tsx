import { useState } from 'react';
import axios from 'axios';
import './CadastroTipoVeiculo.css'; 

const API_BASE_URL = process.env.REACT_APP_API_URL || "http://localhost:5285/api";

//Componente React para cadastrar novos tipos de veículo.

function CadastroTipoVeiculo() {
    const [nomeTipo, setNomeTipo] = useState<string>('');
    const [erro, setErro] = useState<string>('');
    const [sucesso, setSucesso] = useState<string>('');

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();

        setErro('');
        setSucesso('');

        if (!nomeTipo.trim()) {
            setErro("O nome do tipo de veículo é obrigatório.");
            return;
        }

        try {
            const response = await axios.post(`${API_BASE_URL}/TipoVeiculos`, { nome: nomeTipo });

            setSucesso(`Tipo de veículo "${response.data.nome}" cadastrado com sucesso!`);
            setNomeTipo('');

        } catch (error: any) {
            console.error("Erro ao cadastrar tipo de veículo:", error);

            if (error.response && error.response.data && error.response.data.errors) {
                const validationErrors = Object.values(error.response.data.errors).flat();
                setErro(`Erro de validação: ${validationErrors.join(' ')}`);
            } else if (error.response && error.response.data) {
                setErro(`Erro ao cadastrar tipo de veículo: ${error.response.data}`);
            } else {
                setErro("Erro ao cadastrar tipo de veículo. Verifique sua conexão ou tente novamente.");
            }
        }
    };

    return (
        <div className="form-page-container">
            <h1 className="form-page-title">Cadastrar Tipo de Veículo</h1>

            {erro && (
                <div className="alert error-alert" role="alert">
                    <strong>Erro!</strong> {erro}
                </div>
            )}

            {sucesso && (
                <div className="alert success-alert" role="alert">
                    <strong>Sucesso!</strong> {sucesso}
                </div>
            )}

            <form onSubmit={handleSubmit} className="standard-form">
                <div className="form-group">
                    <label htmlFor="nomeTipo">Nome do Tipo de Veículo:</label>
                    <input
                        type="text"
                        id="nomeTipo"
                        value={nomeTipo}
                        onChange={(e) => setNomeTipo(e.target.value)}
                        required
                        maxLength={50}
                        minLength={3}
                        placeholder="Ex: Carro, Moto, Caminhão"
                    />
                </div>
                <div className="form-actions">
                    <button type="submit" className="button submit-button">
                        Cadastrar
                    </button>
                </div>
            </form>
        </div>
    );
}

export default CadastroTipoVeiculo;
