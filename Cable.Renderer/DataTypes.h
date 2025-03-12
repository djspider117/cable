#pragma once
#include <vector>
#include <DirectXMath.h>
#include <msclr/gcroot.h>

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
	};
	public struct GradientMaterial
	{
		XMFLOAT4 Color1;
		XMFLOAT4 Color2;
		GradientMaterialType Type;
		GradientRenderMode RenderMode;
		short Steps;
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
	};
	public struct Mesh2D
	{
		Geometry2D Geometry;
		XMMATRIX Transform;
		Material RenderMaterial;
	};

	public enum class RenderCommandType
	{
		Mesh2D
	};
	public struct Mesh2DRenderCommand
	{
		Mesh2D Mesh;
	};

	public struct RenderCommand
	{
		RenderCommandType Type;

		union
		{
			Mesh2DRenderCommand Mesh2DCommand;
		};
	};
	
	ColorMaterial FromManagedType(Types::ColorMaterialData^ o)
	{
		return ColorMaterial{ XMFLOAT4(o->Color.X, o->Color.Y, o->Color.Z, o->Color.W) };
	}
	GradientMaterial FromManagedType(Types::GradientMaterialData^ o)
	{
		return GradientMaterial{
			XMFLOAT4(o->Color1.X, o->Color1.Y, o->Color1.Z, o->Color1.W),
			XMFLOAT4(o->Color2.X, o->Color2.Y, o->Color2.Z, o->Color2.W),
			(GradientMaterialType)(int)o->Type,
			(GradientRenderMode)(int)o->RenderMode,
			o->Steps
		};
	}
	
	Geometry2D FromManagedType(Types::Geometry2D^ original)
	{
		Geometry2D rv;
		for each(System::Numerics::Vector2 ^ vert in original->Vertices)
		{
			rv.Vertices.push_back(XMFLOAT2(vert->X, vert->Y));
		}

		pin_ptr<int> pin(&original->Indices[0]);
		std::copy(
			static_cast<int*>(pin),
			static_cast<int*>(pin + original->Indices->Length),
			rv.Indicies.begin()
		);

		return rv;
	}
	
	Mesh2D FromManagedType(Types::Mesh2D^ mesh)
	{
		Mesh2D rv;
		rv.Geometry = FromManagedType(mesh->Geometry);

		Matrix3x2^ t = mesh->Transform;
		rv.Transform = XMMATRIX(t->M11, t->M12, 0, 0, t->M21, t->M22, 0, 0, 0, 0, 1, 0, t->M31, t->M32, 0, 1);

		if (mesh->Material->GetType() == Types::ColorMaterialData::typeid)
		{
			Types::ColorMaterialData^ matData = (Types::ColorMaterialData^)mesh->Material;
			rv.RenderMaterial = Material(FromManagedType(matData));
		}

		if (mesh->Material->GetType() == Types::GradientMaterialData::typeid)
		{
			Types::GradientMaterialData^ matData = (Types::GradientMaterialData^)mesh->Material;
			rv.RenderMaterial = Material(FromManagedType(matData));
		}

		return rv;
	}
	/*
	Mesh2DRenderCommand FromManagedType(Types::Mesh2DRenderCommand^ original)
	{
		Mesh2DRenderCommand rv;
		rv.Mesh = FromManagedType(original->Mesh);
		return rv;
	}*/

	//RenderCommand FromManagedType(Types::IRenderCommand^ renderCommand)
	//{
	//	RenderCommand rv;

	//	if (renderCommand->GetType() == Types::Mesh2DRenderCommand::typeid)
	//	{
	//		Types::Mesh2DRenderCommand^ matData = (Types::Mesh2DRenderCommand^)renderCommand;
	//		rv.Mesh2DCommand = FromManagedType(matData);
	//	}

	//	return rv;
	//}
	
}