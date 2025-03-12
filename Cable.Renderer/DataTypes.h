#pragma once
#include <vector>
#include <DirectXMath.h>

using namespace DirectX;
using namespace Cable::Data;
using namespace System::Numerics;

namespace Cable::Data::Types::Native
{
	public enum class GradientMaterialType
	{
		Horizontal,
		Vertical,
		Radial,
		Angular
	};
	public enum class GradientRenderMode
	{
		Smooth,
		Stepped
	};

	public struct ColorMaterial
	{
		XMFLOAT4 Color;

		ColorMaterial()
		{
			Color = XMFLOAT4(1.0f, 0.0f, 1.0f, 1.0f);
		}

		ColorMaterial(Types::ColorMaterialData^ o)
			: Color(XMFLOAT4(o->Color.X, o->Color.Y, o->Color.Z, o->Color.W))
		{

		}
	};
	public struct GradientMaterial
	{
		XMFLOAT4 Color1;
		XMFLOAT4 Color2;
		GradientMaterialType Type;
		GradientRenderMode RenderMode;
		short Steps;

		GradientMaterial() :
			Type(GradientMaterialType::Horizontal),
			RenderMode(GradientRenderMode::Smooth),
			Color1(XMFLOAT4(1.0f, 0.0f, 1.0f, 1.0f)),
			Color2(XMFLOAT4(0.0f, 1.0f, 0.0f, 1.0f))
		{

		}

		GradientMaterial(Types::GradientMaterialData^ o) :
			Color1(XMFLOAT4(o->Color1.X, o->Color1.Y, o->Color1.Z, o->Color1.W)),
			Color2(XMFLOAT4(o->Color2.X, o->Color2.Y, o->Color2.Z, o->Color2.W)),
			Steps(o->Steps),
			Type((GradientMaterialType)(int)o->Type),
			RenderMode((GradientRenderMode)(int)o->RenderMode)
		{
		}

	};

	public enum class MaterialType
	{
		SolidColor,
		Gradient
	};
	public struct Material
	{
		MaterialType Type;

		union
		{
			ColorMaterial Color;
			GradientMaterial Gradient;
		};

		Material() : Type(MaterialType::SolidColor), Color{} {}
		Material(ColorMaterial mat) : Type(MaterialType::SolidColor), Color(mat) {}
		Material(GradientMaterial mat) : Type(MaterialType::Gradient), Gradient(mat) {}
	};

	public struct Geometry2D
	{
		std::vector<int> Indicies;
		std::vector<XMFLOAT2> Vertices;

		Geometry2D(Types::Geometry2D^ original);
	};
	public struct Mesh2D
	{
		Geometry2D Geometry;
		XMMATRIX Transform;
		Material RenderMaterial;

		Mesh2D(Types::Mesh2D^ mesh) :
			Geometry(mesh->Geometry),
			Transform()
		{
			Matrix3x2^ t = mesh->Transform;
			Transform = XMMATRIX(t->M11, t->M12, 0, 0, t->M21, t->M22, 0, 0, 0, 0, 1, 0, t->M31, t->M32, 0, 1);

			if (mesh->Material->GetType() == Types::ColorMaterialData::typeid)
			{
				Types::ColorMaterialData^ matData = (Types::ColorMaterialData^)mesh->Material;
				RenderMaterial = Material(ColorMaterial(matData));
				return;
			}

			if (mesh->Material->GetType() == Types::GradientMaterialData::typeid)
			{
				Types::GradientMaterialData^ matData = (Types::GradientMaterialData^)mesh->Material;
				RenderMaterial = Material(GradientMaterial(matData));
				return;
			}
		}
	};

	public enum class RenderCommandType
	{
		Mesh2D
	};
	public struct Mesh2DRenderCommand
	{
		Mesh2D Mesh;

		Mesh2DRenderCommand(Types::Mesh2DRenderCommand^ original) : Mesh(original->Mesh) {}
	};

	public struct RenderCommand
	{
		RenderCommandType Type;

		union
		{
			Mesh2DRenderCommand Mesh2DCommand;
		};

		RenderCommand(Types::IRenderCommand^ renderCommand)
		{
			if (renderCommand->GetType() == Types::Mesh2DRenderCommand::typeid)
			{
				Types::Mesh2DRenderCommand^ matData = (Types::Mesh2DRenderCommand^)renderCommand;
				Mesh2DCommand = Mesh2DRenderCommand(matData);
				return;
			}
		}
	};
}