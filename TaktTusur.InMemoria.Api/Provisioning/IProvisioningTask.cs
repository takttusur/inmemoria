namespace TaktTusur.InMemoria.Api.Provisioning;

/// <summary>
/// Provisioning task interface.
/// The Task is responsible for validating current configuration and adjust the behaviour.
/// </summary>
public interface IProvisioningTask
{
	void Provision();
}
